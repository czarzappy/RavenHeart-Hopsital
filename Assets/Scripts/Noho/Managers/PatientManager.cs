using Noho.Messages;
using Noho.Parsing.Models;
using UnityEngine;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Managers
{
    public class PatientManager
    {
        public int PatientId;
        public PatientDef PatientDef;

        public float CurrentVitals;
        
        public PhaseManager PhaseManager;

        public float VitalDamagePerSecond = 1f / 5;

        #region Properties

        public int RoundUpVitals => Mathf.CeilToInt(CurrentVitals);
        
        public string DisplayCurrentVitals
        {
            get
            {
                int capped =  Mathf.Max(RoundUpVitals, 0);
                return $"{capped:D0}";
            }
        }
        
        public float VitalPercentage => CurrentVitals / PatientDef.Vitals.Max;
        

        #endregion
        
        public void Init()
        {
            PhaseManager = new PhaseManager();
            
            PhaseManager.Init();
        }

        public void LoadItem(PatientDef patient)
        {
            ZBug.Info("PATIENT", $"Loading patient: {patient} (Id: {patient.Id})");
            PatientId = patient.Id;
            PatientDef = patient;

            CurrentVitals = patient.Vitals.Starting;

            PhaseManager.LoadItems(patient.PhaseDefs);
        }

        public PhaseManager.PhaseMode MoveNextPhase()
        {
            return PhaseManager.MoveNextPhase();
        }

        public void MoveNextAction()
        {
            ZBug.Info("PATIENT", "Moving to next action");
            PhaseManager.MoveNextAction();
        }

        public void Damage(float amount)
        {
            CurrentVitals -= amount;
            
            Send.Msg(new PatientVitalsChangedMsg
            {
                PatientId = PatientId,
                NewDisplayVitals = DisplayCurrentVitals,
                NewVitalPercentage = VitalPercentage,
                NewVitals = RoundUpVitals
            });
        }

        public void Heal(float amount)
        {
            CurrentVitals += amount;

            CurrentVitals = Mathf.Min(CurrentVitals, PatientDef.Vitals.Max);
            
            Send.Msg(new PatientVitalsChangedMsg
            {
                PatientId = PatientId,
                NewDisplayVitals = DisplayCurrentVitals,
                NewVitalPercentage = VitalPercentage,
                NewVitals = RoundUpVitals
            });
        }

        public void Tick(float deltaTime)
        {
            Damage(VitalDamagePerSecond * deltaTime);
            
            if (RoundUpVitals <= -1)
            {
                Send.Msg(new OperationFailedMsg());
                return;
            }
        }
    }
}