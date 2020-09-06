using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ZEngine.Unity.Core.Extensions;

namespace ZEngine.Unity.Core.Attributes
{
    public class DragAttr : InitZAttr<DragAttr.InitData>, IPointerDownHandler, IPointerUpHandler
    {
        public struct InitData
        {
            public Func<bool> Condition;
            public Action OnDragStart;
            public Action OnDragStop;
        }

        protected override void PostInit()
        {
        }

        public Vector2 pressPos_World;

        public DragState State = DragState.NONE;

        public enum DragState
        {
            NONE,
            DRAGGING
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ZBug.Info("ZATTR", $"{this} PointerDown!");
            
            if (initData.Condition == null || initData.Condition())
            {
                pressPos_World = eventData.pointerCurrentRaycast.worldPosition;
                
                rectTransform.WorldToCanvasSpace(pressPos_World);
                StartDrag(pressPos_World);
            }
        }

        private ZAttr mMouseAttr;

        private void StartDrag(Vector2 pressWorldPos)
        {
            ZBug.Info("ZATTR", $"{this} Start Drag!");
            
            State = DragState.DRAGGING;
            Vector2 currWorldPos = rectTransform.position;
            
            mMouseAttr = gameObject.InitZAttr<UIPointerFollowZAttr, UIPointerFollowZAttr.InitData>(
                new UIPointerFollowZAttr.InitData
                {
                    WorldPosOffset = pressWorldPos - currWorldPos
                });

            initData.OnDragStart?.Invoke();
        }

        public void ForceDragStop()
        {
            ZBug.Info("ZATTR", $"{this} Force Drag Stop!");
            if (mMouseAttr != null)
            {
                gameObject.RemoveZAttr(mMouseAttr);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            State = DragState.NONE;
            ForceDragStop();

            initData.OnDragStop?.Invoke();
        }
    }
}