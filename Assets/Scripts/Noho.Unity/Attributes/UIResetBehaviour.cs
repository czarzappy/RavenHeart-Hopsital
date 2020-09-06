using System;
using Noho.Unity.Models;
using UnityEngine;
using ZEngine.Unity.Core.Attributes;
using ZEngine.Unity.Core.Components;

namespace Noho.Unity.Attributes
{
    public class UIResetBehaviour : UIMonoBehaviour
    {
        public Vector2 ResetPos;

        public event Action OnReset;

        public void UpdateResetPos()
        {
            ZBug.Info("ZATTR", $"{this.gameObject} [UIResetBehaviour] Setting reset pos: {ResetPos}");
            ResetPos = rectTransform.anchoredPosition;
        }

        public void Reset()
        {
            ZBug.Info("ZATTR",$"{this.gameObject} [UIResetBehaviour] Resetting!");
            gameObject.InitDurationTickZAttr<UITranslateToZAttr, UITranslateToZAttr.InitData>(
                new UITranslateToZAttr.InitData
                {
                    EndPos = ResetPos
                }, NohoUISettings.GENERAL_FAIL_RESET_DURATION);
            
            OnReset?.Invoke();
        }
    }
}