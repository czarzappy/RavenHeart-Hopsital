using Noho.Models;
using Noho.Unity.Messages;
using UnityEngine;
using ZEngine.Unity.Core.Attributes;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery.Tools
{
    public class InjectionToolController : UIMonoBehaviour
    {
        public enum InjectionPhase
        {
            EMPTY,
            FILLED
        }

        public InjectionPhase CurrentPhase = InjectionPhase.FILLED;
        
        public void Start()
        {
            Init();
        }

        private void Init()
        {
            gameObject.AddZAttr<UIPointerFollowZAttr>();
        }

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!ToolboxController.Instance.IsEquipped(NohoConstants.ToolType.INJECTION))
                {
                    return;
                }
            
                BrainMain.Instance.Context.OperationManager.CurrentPatientManager.Heal(20);
            
            
                Send.Msg(new InjectedMsg
                {
                });
            }
        }
    }
}