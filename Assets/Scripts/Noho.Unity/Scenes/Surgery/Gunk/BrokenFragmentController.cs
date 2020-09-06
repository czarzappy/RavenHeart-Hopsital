using Noho.Configs;
using Noho.Unity.Attributes;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using UnityEngine;
using ZEngine.Unity.Core.Attributes;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    public class BrokenFragmentController : UIMonoBehaviour
    {
        public Quaternion OriginalRotation;

        public enum FragmentPhase
        {
            CLEANUP,
            PLACE_BACK,
            PLACED
        }

        public FragmentPhase Phase;

        private TrayGunkDragAttr mTrayGunkDragAttr;

        private Vector3 mGoalPos;

        private GoalGunkDragAttr mGoalGunkDragAttr;
        public string ParentTag;
        
        public void Init(string parentTag)
        {
            ParentTag = parentTag;
            
            Phase = FragmentPhase.CLEANUP;

            OriginalRotation = rectTransform.rotation;

            mGoalPos = rectTransform.position;
        }

        public void Init2()
        {
            
            mTrayGunkDragAttr = gameObject.InitZAttr<TrayGunkDragAttr, TrayGunkDragAttr.InitData>(new TrayGunkDragAttr.InitData
            {
                OnGunkCleared = OnGunkCleared
            });
        }
        
        
        private void OnGunkCleared()
        {
            Phase = FragmentPhase.PLACE_BACK;
            gameObject.RemoveZAttr(mTrayGunkDragAttr);
            
            mGoalGunkDragAttr = gameObject.InitZAttr<GoalGunkDragAttr, GoalGunkDragAttr.InitData>(new GoalGunkDragAttr.InitData
            {
                GoalPos = mGoalPos,
                OnGoalPlaced = OnGoalPlaced
            });
            
            transform.rotation = OriginalRotation;
            Earn.Score(ParentTag);
            Send.Msg(new FragmentClearedMsg
            {
                Fragment = gameObject
            });
            Send.Msg(new StartNewProcedureStepMsg
            {
                Step = ProcedureStep.STEP_2,
                Tag = ParentTag
            });
        }

        private void OnGoalPlaced()
        {
            gameObject.RemoveZAttr(mGoalGunkDragAttr);
            Phase = FragmentPhase.PLACED;

            Earn.Score(ParentTag);
            Send.Msg(new FragmentPlacedMsg
            {
                Fragment = gameObject
            });
        }
    }
}