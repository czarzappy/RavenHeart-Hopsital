using System;
using UnityEngine;

namespace ZEngine.Unity.Core.Attributes
{
    public class TranslateUIZAttr : InitDurationTickZAttr<TranslateUIZAttr.InitData>
    {
        [Serializable]
        public struct InitData
        {
            public Vector2 StartAnchoredPos;
            public Vector2 EndAnchoredPos;
        }
        
        // Movement speed in units per second.
        // private readonly float Speed = 10.0F;
        
        private float mTotalDisplacement;

        // private RectTransform mRectTransform;
        protected override void PostInit()
        {
            ZBug.Info("ZATTR", $"{this.gameObject} [TranslateUI] From: {initData.StartAnchoredPos} to {initData.EndAnchoredPos}");
            // Calculate the journey length.
            mTotalDisplacement = Vector3.Distance(initData.StartAnchoredPos, initData.EndAnchoredPos);

            rectTransform.anchoredPosition = initData.StartAnchoredPos;
        }

        public override void Tick(float t)
        {
            // Set our position as a fraction of the distance between the markers.
            rectTransform.anchoredPosition = Vector3.Lerp(initData.StartAnchoredPos, initData.EndAnchoredPos, t);
        }
    }
}