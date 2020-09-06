using System;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Dev
{
    public class ToolMain : UIMonoBehaviour
    {
        public void FixedUpdate()
        {
            MsgMgr.Instance.Pump();
        }
    }
}