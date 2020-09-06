using Noho.Messages;
using Noho.Unity.Messages;
using UnityEngine;
using UnityEngine.UI.UIShape;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Math;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery.Tools
{
    public class SyringeController : UIMonoBehaviour
    {
        public UIShape UIShape;

        public float Amplitude = 40f;
        public float Offset = 20f;
        public float Speed = 1f;

        public void Awake()
        {
            Init();
        }

        public void OnDestroy()
        {
            UnInit();
        }

        private void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<InjectedMsg>(OnInjected);
        }

        private void Init()
        {
            MsgMgr.Instance.SubscribeTo<InjectedMsg>(OnInjected);
        }

        private void OnInjected(InjectedMsg message)
        {
            Send.Msg(new GunkClearedMsg
            {
                Gunk = this
            });
        }

        public void Update()
        {
            UIShape.Thickness = Amplitude * ZMath.Cos1To0(Time.time * Speed) + Offset;

            // if (Input.GetMouseButtonDown(0))
            // {
            //     if (!ToolboxController.Instance.IsEquipped(NohoConstants.ToolType.INJECTION))
            //     {
            //         return;
            //     }
            //
            //     BrainMain.Instance.Context.OperationManager.CurrentPatientManager.Heal(20);
            //
            //
            // }
        }

        // public void OnTriggerEnter2D(Collider2D other)
        // {
        //     
        // }
    }
}