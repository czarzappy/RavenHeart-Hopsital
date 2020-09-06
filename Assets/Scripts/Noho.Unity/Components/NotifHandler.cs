using System;
using Noho.Unity.Messages;
using TMPro;
using UnityEngine;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Components
{
    public class NotifHandler : UIMonoBehaviour
    {
        public TMP_Text Text;
        public CanvasGroup CanvasGroup;
        public void Awake()
        {
            Init();
        }
        public void Init()
        {
            MsgMgr.Instance.SubscribeTo<NewNotifMsg>(OnNewNotifMsg);
        }

        public void OnDestroy()
        {
            UnInit();
        }

        public void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<NewNotifMsg>(OnNewNotifMsg);
        }

        public float DisplayStartTime;
        public float DisplayTime = 1f;
        public float DisplayFade = 1f;

        public bool HasText = false;
        private void OnNewNotifMsg(NewNotifMsg message)
        {
            HasText = true;
            Text.text = message.Notif;
            DisplayStartTime = Time.time;
        }

        public void Update()
        {
            if (!HasText)
            {
                CanvasGroup.alpha = 0;
                return;
            }
            
            float offsetTime = Time.time - (DisplayStartTime + DisplayTime);
            if (offsetTime <= 0f)
            {
                CanvasGroup.alpha = 1;
                return;
            }

            if (offsetTime > DisplayFade)
            {
                CanvasGroup.alpha = 0;
                return;
            }

            CanvasGroup.alpha = Mathf.Lerp(1, 0, offsetTime / DisplayFade);
        }
    }
}