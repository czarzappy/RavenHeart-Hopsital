using System;
using Noho.Managers;
using Noho.Models;
using Noho.Parsing.Models;
using UnityEngine;
using ZEngine.Core.Dialogue.Models;
using ZEngine.Unity.Core;

namespace Noho.Messages
{
    public class GunkClearedMsg : BaseNohoMessage
    {
        public MonoBehaviour Gunk;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.CLEARED_GUNK;
        public bool KeepAfterClear;
    }
    
    public class DefibAttemptMsg : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.DEFIB_ATTEMPT;
    }

    [Serializable]
    public class GunkInitData
    {
        [TagSelector]
        public string GunkTag;
        
        public int Amount;
    }

    [Serializable]
    public class GunkInitIndicesData
    {
        [TagSelector]
        public string GunkTag;
        
        public int[] Indices;
    }
    
    public class InitGunkMsg : BaseNohoMessage
    {
        public GunkInitData[] InitData;

        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.INIT_GUNK;
    }
    
    public class InitGunkIndicesMsg : BaseNohoMessage
    {
        public GunkInitIndicesData[] InitData;

        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.INIT_GUNK_INDICES;
    }

    public class ClearedAllGunkMsg : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.PHASE_END;
    }

    public class LoadNewCutSceneMsg : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.LOAD_NEW_CUT_SCENE;
        public CutSceneDef NewCutScene;
    }

    public class LoadNewOperationSceneMsg : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.LOAD_NEW_OPERATION_SCENE;
        public OperationDef NewOperationScene;
    }

    public class LoadPostOpSceneMsg : BaseNohoMessage
    {
        public PostOpDef PostOpDef;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.LOAD_POST_OP_SCENE;
    }
    
    public class OperationLoadedMsg : BaseNohoMessage
    {
        public OperationDef Operation;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.OPERATION_LOADED;
    }
    public class OperationCompletedMsg : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.OPERATION_COMPLETED;
    }

    public class ScriptCompleteMsg : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.SCRIPT_COMPLETE;
    }

    public class CutSceneLoadedMsg : BaseNohoMessage
    {
        public CutSceneDef CutScene;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.CUT_SCENE_LOADED;
    }

    public class CutSceneCompletedMsg : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.CUT_SCENE_COMPLETED;
    }
    
    public class LoadNewSurgerySceneMsg : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.LOAD_NEW_SCENE;
        public string SceneName;
    }
    
    public class OperationFailedMsg : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.OPERATION_FAILED;
    }
    
    public class NewSceneLoadedMsg : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.NEW_SCENE_LOADED;
    }

    public class PhaseSetupCompleted : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType =>
            MessageConstants.NohoMsgType.PHASE_SETUP_COMPLETED;
    }
    
    public class StageActionsCompletedMsg : BaseNohoMessage
    {
        public StageActionManager Sender;

        public override MessageConstants.NohoMsgType NohoMsgType =>
            MessageConstants.NohoMsgType.STAGE_ACTIONS_COMPLETED;
    }

    public class PhaseCompletedMsg : BaseNohoMessage
    {
        public PhaseManager Sender;

        public override MessageConstants.NohoMsgType NohoMsgType =>
            MessageConstants.NohoMsgType.PHASE_COMPLETED;
    }

    public class PhasesCompletedMsg : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.PHASES_COMPLETE;
    }

    public class PatientCompleteMsg : BaseNohoMessage
    {
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.PATIENT_COMPLETE;
    }

    public class NewStageActionLoadedMsg : BaseNohoMessage
    {
        public StageAction NewStageAction;

        public override MessageConstants.NohoMsgType NohoMsgType =>
            MessageConstants.NohoMsgType.NEW_STAGE_ACTION_LOADED;
    }
    

    public class PatientVitalsChangedMsg : BaseNohoMessage
    {
        public int PatientId;

        public string NewDisplayVitals;
        public float NewVitalPercentage;
        public int NewVitals;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.PATIENT_VITALS_CHANGED;
    }

    public class CurrentPatientChangedMsg : BaseNohoMessage
    {
        public int NewPatientId;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.CURRENT_PATIENT_CHANGED;
    }

    public class NewPatientMsg : BaseNohoMessage
    {
        public int NewPatientId;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.CURRENT_PATIENT_CHANGED;
    }

    public class NewToolTipMsg : BaseNohoMessage
    {
        public string ToolTip;
        public override MessageConstants.NohoMsgType NohoMsgType => MessageConstants.NohoMsgType.CURRENT_PATIENT_CHANGED;
    }
}