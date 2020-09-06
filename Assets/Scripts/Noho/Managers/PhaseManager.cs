using System.Collections.Generic;
using Noho.Messages;
using Noho.Parsing.Models;
using ZEngine.Core.Dialogue.Models;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Managers
{
    public class PhaseManager
    {
        public int CurrentPhaseIdx = 0;

        public List<PhaseDef>.Enumerator Phases;

        public string CurrentOperationSceneName;

        public StageActionManager StageActionManager;

        public void Init()
        {
            StageActionManager = new StageActionManager();
        }

        private void OnStageActionsComplete()
        {
        }

        public void LoadItems(List<PhaseDef> phaseDefs)
        {
            Phases = phaseDefs.GetEnumerator();
        }

        public void MoveNextAction()
        {
            ZBug.Info("PHASE", "Moving to next action");
            if (!StageActionManager.MoveNextAction())
            {
                ZBug.Warn("PHASE", "Complete!");
                Send.Msg(new PhaseSetupCompleted
                {
                
                });
            }
        }

        public enum PhaseMode
        {
            NO_PHASES,
            NEW_SCENE,
            NEXT_PHASE,
        }
        public PhaseMode MoveNextPhase()
        {
            if (!Phases.MoveNext())
            {
                ZBug.Warn("PHASE", "No phases left");
                return PhaseMode.NO_PHASES;
            }

            var current = Phases.Current;
                
            StageActionManager.LoadItems(current.Actions);

            if (!string.IsNullOrEmpty(current.SurgerySceneName) && 
                current.SurgerySceneName != CurrentOperationSceneName)
            {
                ZBug.Warn("PHASE", $"Found new scene to load: {current.SurgerySceneName}");
                CurrentOperationSceneName = current.SurgerySceneName;
                    
                Send.Msg(new LoadNewSurgerySceneMsg
                {
                    SceneName = CurrentOperationSceneName
                });
                return PhaseMode.NEW_SCENE;
            }

            ZBug.Info("PHASE", $"Moving to next phase, current scene: {CurrentOperationSceneName}");
            return PhaseMode.NEXT_PHASE;
        }
    }
}