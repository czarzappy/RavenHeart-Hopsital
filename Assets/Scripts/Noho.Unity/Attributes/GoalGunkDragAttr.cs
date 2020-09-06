using System;
using Noho.Models;
using Noho.Unity.Models;
using Noho.Unity.Scenes.Surgery;
using UnityEngine;
using ZEngine.Unity.Core.Attributes;

namespace Noho.Unity.Attributes
{
    public class GoalGunkDragAttr : InitZAttr<GoalGunkDragAttr.InitData>
    {
        public struct InitData
        {
            public Vector3 GoalPos;
            public Action OnGoalPlaced;
        }

        private const float MIN_GOAL_DISTANCE = NohoUISettings.MIN_FRAGMENT_GOAL_DISTANCE;

        private UIResetBehaviour mUIResetBehaviour;
        private DragAttr mDragAttr;

        protected override void PostInit()
        {
            mUIResetBehaviour = gameObject.AddComponent<UIResetBehaviour>();
            
            mDragAttr = gameObject.InitZAttr<DragAttr, DragAttr.InitData>(new DragAttr.InitData
            {
                Condition = ToolboxController.IsEquippedCB(NohoConstants.ToolType.FORCEPS),
                OnDragStart = mUIResetBehaviour.UpdateResetPos,
                OnDragStop = OnPlaceBackDragStop,
            });
        }

        private void OnPlaceBackDragStop()
        {
            float dist = Vector2.Distance(rectTransform.position, initData.GoalPos);
            ZBug.Info($"dist: {dist}");
            if (dist <= MIN_GOAL_DISTANCE)
            {
                rectTransform.position = initData.GoalPos;
                gameObject.RemoveZAttr(mDragAttr);

                initData.OnGoalPlaced();
            }
            else
            {
                mUIResetBehaviour.Reset();
            }
        }
    }
}