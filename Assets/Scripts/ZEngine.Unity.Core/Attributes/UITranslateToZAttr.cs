using System;
using UnityEngine;

namespace ZEngine.Unity.Core.Attributes
{
    public class UITranslateToZAttr : InitDurationTickZAttr<UITranslateToZAttr.InitData>
    {
        [Serializable]
        public struct InitData
        {
            public Vector2 EndPos;
        }
        
        // Movement speed in units per second.
        // private readonly float Speed = 10.0F;
        
        private float mTotalDisplacement;

        private Vector2 mStartPos;

        protected override void PostInit()
        {
            mStartPos = rectTransform.anchoredPosition;
            // Calculate the journey length.
            mTotalDisplacement = Vector3.Distance(mStartPos, initData.EndPos);
        }

        public override void Tick(float t)
        {
            // Set our position as a fraction of the distance between the markers.
            rectTransform.anchoredPosition = Vector2.Lerp(mStartPos, initData.EndPos, t);
        }
    }
}