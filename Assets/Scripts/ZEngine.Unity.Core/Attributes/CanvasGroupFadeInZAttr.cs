using System;
using UnityEngine;

namespace ZEngine.Unity.Core.Attributes
{
    public class CanvasGroupFadeInZAttr : InitFiniteTickZAttr<CanvasGroupFadeInZAttr.InitData>
    {
        [Serializable]
        public struct InitData
        {
            public CanvasGroup CanvasGroup;
            public float Duration;
        }

        private float mStart;
        private float mEnd;
        
        protected override void PostInit()
        {
            mStart = 0f;
            mEnd = 1f;
            initData.CanvasGroup.alpha = 0f;
        }

        public override float T => ElapsedTime / initData.Duration;
        private float mX;

        public override void Tick(float t)
        {
            mX = Mathf.Lerp(mStart, mEnd, t);
            initData.CanvasGroup.alpha = mX;
        }
    }
}