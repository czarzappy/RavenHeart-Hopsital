using System.Collections;
using System.Collections.Generic;
using Noho.Messages;
using Noho.Models;
using Noho.Parsing.Models;
using UnityEngine;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Managers
{
    public class OperationManager
    {
        public const float NEXT_PHASE_DELAY = .75f;
        
        public readonly List<PatientManager> PatientManagers = new List<PatientManager>();

        public readonly List<int> ActivePatientIds = new List<int>();
        public readonly OperationScore Score = new OperationScore();
        
        public int CurrentActivePatientIdx;
        private bool mIsComplete;
        
        
        public OperationDef OperationDef;
        

        public float ElapsedSeconds = 0;

        private bool mTransitioning = false;

        #region Properties
        public bool CanTick
        {
            get
            {
                if (mTransitioning)
                {
                    return false;
                }
                
                return true;
            }
        }

        public int CurrentPatientId => ActivePatientIds[CurrentActivePatientIdx];
        public PatientManager CurrentPatientManager => GetPatient(CurrentPatientId);
        public int NumPatients => PatientManagers.Count;

        #endregion

        private void Reset()
        {
            ElapsedSeconds = 0;
            mIsComplete = false;
            CurrentActivePatientIdx = 0;
            
            PatientManagers.Clear();
            ActivePatientIds.Clear();

            OperationDef = null;
            
            Score.FullReset();
        }
        
        public void LoadItem(OperationDef operationDef)
        {
            Reset();
            OperationDef = operationDef;

            foreach (int patientId in operationDef.InitialPatientIds)
            {
                ActivatePatient(patientId);
            }

            foreach (PatientDef patient in OperationDef.Patients)
            {
                PatientManager patientManager = new PatientManager();

                patientManager.Init();
                patientManager.LoadItem(patient);
                
                PatientManagers.Add(patientManager);
            }
            
            Send.Msg(new OperationLoadedMsg
            {
                Operation = operationDef,
            });
        }

        private void ActivatePatient(int patientId)
        {
            ActivePatientIds.Add(patientId);
            
            Send.Msg(new NewPatientMsg
            {
                NewPatientId = patientId
            });
        }

        public void Tick(float deltaTime)
        {
            if (!CanTick)
            {
                return;
            }
            
            foreach (PatientManager patientManager in PatientManagers)
            {
                if (!ActivePatientIds.Contains(patientManager.PatientId))
                {
                    continue;
                }

                patientManager.Tick(deltaTime);
            }
            
            Score.Tick(deltaTime);

            ElapsedSeconds += deltaTime;
        }

        public PhaseManager.PhaseMode MoveNextPhase()
        {
            return CurrentPatientManager.MoveNextPhase();
        }
        
        public void MoveNextAction()
        {
            ZBug.Info("OPERATION", "Moving to next action");
            CurrentPatientManager.MoveNextAction();
        }

        public IEnumerator MoveNext()
        {
            ZBug.Info("OPERATION", "Moving next");
            if (mIsComplete)
            {
                // ZBug.Info("OPERATION", "Already completed, sending message");
                // Send.Msg(new OperationCompletedMsg());
                yield break;
            }
            
            var mode = MoveNextPhase();
            switch (mode)
            {
                case PhaseManager.PhaseMode.NEXT_PHASE:

                    mTransitioning = true;
                    yield return new WaitForSeconds(NEXT_PHASE_DELAY);
                    
                    MoveNextAction();
                    mTransitioning = false;
                    break;
                
                case PhaseManager.PhaseMode.NO_PHASES:
                    mIsComplete = true;
                    
                    yield return new WaitForSeconds(NEXT_PHASE_DELAY);
                    
                    ZBug.Info("OPERATION", "All phased completed");
                    Send.Msg(new OperationCompletedMsg());
                    break;
            }
        }

        public void ShiftActivePatient(int shiftAmount)
        {
            CurrentActivePatientIdx = (CurrentActivePatientIdx + shiftAmount + ActivePatientIds.Count) % ActivePatientIds.Count;
            
            Send.Msg(new CurrentPatientChangedMsg
            {
                NewPatientId = CurrentPatientManager.PatientId
            });
        }

        public PatientManager GetPatient(int patientId)
        {
            foreach (PatientManager patientManager in PatientManagers)
            {
                if (patientManager.PatientId == patientId)
                {
                    return patientManager;
                }
            }

            ZBug.Warn("PATIENT", $"Attempted to get missing patient id: {patientId}");
            return null;
        }
    }
}