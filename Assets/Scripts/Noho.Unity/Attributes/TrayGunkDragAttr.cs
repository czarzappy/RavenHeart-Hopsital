using System;
using Noho.Models;
using Noho.Unity.Scenes.Surgery;
using UnityEngine;
using ZEngine.Unity.Core.Attributes;

namespace Noho.Unity.Attributes
{
    /// <summary>
    /// Expects game object to have a 2D Collider
    /// </summary>
    public class TrayGunkDragAttr : InitZAttr<TrayGunkDragAttr.InitData>
    {
        public struct InitData
        {
            public Action<Collider2D> OnTriggerEnterUnknown2D;
            public Action OnGunkCleared;
            public Action OnReset;
        }
        
        private bool mFailed = true;
        
        private DragAttr mDragAttr;

        public UIResetBehaviour UIResetBehaviour;

        protected override void PostInit()
        {
            UIResetBehaviour = this.gameObject.AddComponent<UIResetBehaviour>();
            UIResetBehaviour.UpdateResetPos();
            UIResetBehaviour.OnReset += initData.OnReset;
            
            mDragAttr = this.gameObject.InitZAttr<DragAttr, DragAttr.InitData>(
                new DragAttr.InitData
                {
                    // Gunk has to be picked up with forceps
                    Condition = ToolboxController.IsEquippedCB(NohoConstants.ToolType.FORCEPS),
                    OnDragStop = OnDragStop,
                });
        }

        private void OnDragStop()
        {
            ZBug.Info("TrayGunkDragAttr", $"{this.gameObject} On Drag Stop!");
            if (mFailed)
            {
                ZBug.Info("TrayGunkDragAttr", $"{this.gameObject} Failed!");
                UIResetBehaviour.Reset();
            }
            else
            {
                ZBug.Info("TrayGunkDragAttr", $"{this.gameObject} Taking gunk away");
                // success
                mTrayController.TakeAwayGunk(gameObject, initData.OnGunkCleared);

                // initData.OnGunkCleared?.Invoke();
            }
        }

        private TrayController mTrayController;


        public void OnTriggerExit2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case NohoConstants.GOTags.TRAY:
                    mFailed = true;

                    mTrayController = null;
                    break;
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case NohoConstants.GOTags.TRAY:
                    ZBug.Info("TrayGunkDragAttr", $"{this.gameObject} triggered by TRAY!");
                    mFailed = false;

                    mTrayController = other.gameObject.GetComponent<TrayController>();
                    break;
                default:
                    initData.OnTriggerEnterUnknown2D?.Invoke(other);
                    break;
            }
        }

        public void ForceReset()
        {
            mDragAttr.ForceDragStop();
                        
            UIResetBehaviour.Reset();
        }
    }
}