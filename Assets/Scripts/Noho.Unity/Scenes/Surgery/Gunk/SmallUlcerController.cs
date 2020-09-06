using Noho.Messages;
using Noho.Models;
using Noho.Unity.Scenes.Surgery.Tools;
using UnityEngine.EventSystems;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    public class SmallUlcerController : UIMonoBehaviour, IPointerDownHandler, IPointerClickHandler
    {
        public void Start()
        {
            Init();
        }

        private void Init()
        {
            ZBug.Info("GUNK", "[ULCER] Init!");
            mIsCleared = false;
        }

        private bool mIsCleared;

        public void OnPointerDown(PointerEventData eventData)
        {
            ZBug.Info("GUNK", "[ULCER] Pointer down!");
            if (!ToolboxController.Instance.IsEquipped(NohoConstants.ToolType.INJECTION))
            {
                return;
            }
            
            ZBug.Info("GUNK", "[ULCER] Injection phase check!");
            if (ToolboxController.Instance.Injection.CurrentPhase == InjectionToolController.InjectionPhase.FILLED)
            {
                if (!mIsCleared)
                {
                    mIsCleared = true;
                    Send.Msg(new GunkClearedMsg
                    {
                        Gunk = this,
                    });
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ZBug.Info("GUNK", "[ULCER] Pointer click!");
        }
    }
}