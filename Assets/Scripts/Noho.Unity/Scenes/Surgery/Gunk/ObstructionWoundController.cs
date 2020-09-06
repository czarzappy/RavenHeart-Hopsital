using Noho.Configs;
using Noho.Messages;
using Noho.Unity.Components;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using UnityEngine;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    public class ObstructionWoundController : MonoBehaviour
    {
        public ObstructionController Obstruction;
        
        public SmallCutController SmallCut;

        public MaskController Mask;
        public bool IsWoundObstructed;

        public ProcedureStep CurrentStep;

        public void Awake()
        {
            Init();
        }

        public void Init()
        {
            MsgMgr.Instance.SubscribeTo<ClearedSmallCutMsg>(OnClearedSmallCut);
            MsgMgr.Instance.SubscribeTo<SmallCutUnobstructedMsg>(OnSmallCutUnobstructed);
            MsgMgr.Instance.SubscribeTo<SmallCutObstructedMsg>(OnSmallCutObstructed);
            
            SmallCut.Init(Obstruction.gameObject);
            Obstruction.Init(OnObstructionReset);
        }

        private void OnObstructionReset()
        {
            if (!SmallCut.IsObstructed)
            {
                SmallCut.AddObstruction();
            }
        }

        private void OnSmallCutUnobstructed(SmallCutUnobstructedMsg message)
        {
            if (message.SmallCut != SmallCut)
            {
                return;
            }
            
            Mask.TurnOffMask();
            IsWoundObstructed = false;

            CurrentStep = ProcedureStep.STEP_2;
            Show.Nice(this.transform);
            Earn.Score(this.tag);
            Send.Msg(new StartNewProcedureStepMsg
            {
                Tag = this.tag,
                Step = ProcedureStep.STEP_2
            });
        }

        private void OnSmallCutObstructed(SmallCutObstructedMsg message)
        {
            if (message.SmallCut != SmallCut)
            {
                return;
            }
            
            Mask.TurnOnMask();
            IsWoundObstructed = true;
            
            CurrentStep = ProcedureStep.STEP_1;
            Send.Msg(new StartNewProcedureStepMsg
            {
                Tag = this.tag,
                Step = ProcedureStep.STEP_1
            });
        }

        private void OnClearedSmallCut(ClearedSmallCutMsg message)
        {
            if (message.SmallCut != SmallCut)
            {
                return;
            }

            CurrentStep = ProcedureStep.DONE;
            
            Send.Msg(new GunkClearedMsg
            {
                Gunk = this
            });
        }
        
    }
}
