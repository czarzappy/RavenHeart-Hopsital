using System;
using Noho.Messages;
using Noho.Models;
using Noho.Unity.Attributes;
using UnityEngine;
using ZEngine.Unity.Core.Attributes;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    public class ObstructionController : MonoBehaviour
    {
        // public CanvasGroup CanvasGroup;

        public bool ClearGunk = false;
        private bool mInit = false;
        public void Start()
        {
            if (!mInit)
            {
                Init(null);
            }
        }

        private TrayGunkDragAttr mTrayGunkDragAttr;

        public void Init(Action onReset)
        {
            mInit = true;
            
            // CanvasGroup.
            mTrayGunkDragAttr = gameObject.InitZAttr<TrayGunkDragAttr, TrayGunkDragAttr.InitData>(
                new TrayGunkDragAttr.InitData
                {
                    OnTriggerEnterUnknown2D = OnTriggerEnterUnknown2D,
                    OnReset = onReset,
                    OnGunkCleared = OnGunkCleared
                });
        }

        private void OnGunkCleared()
        {
            if (ClearGunk)
            {
                Send.Msg(new GunkClearedMsg
                {
                    Gunk = this
                });
            }
        }

        private void OnTriggerEnterUnknown2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case NohoConstants.GOTags.FAILBOUND:

                    if (IsMyWoundFailBound(other.gameObject))
                    {
                        mTrayGunkDragAttr.ForceReset();
                    }
                    
                    break;
            }
        }

        private bool IsMyWoundFailBound(GameObject failBound)
        {
            ObstructionWoundController obstructionWound = failBound.GetComponentInParent<ObstructionWoundController>();

            if (obstructionWound == null)
            {
                return false;
            }

            if (!obstructionWound.IsWoundObstructed)
            {
                return false;
            }
            
            return obstructionWound.Obstruction == this;
        }
    }
}
