using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using ZEngine.Unity.Core.Extensions;

namespace ZEngine.Unity.Core.Attributes
{
    public class AutoScrollerZAttr : InitFiniteTickZAttr<AutoScrollerZAttr.InitData>
    {
        [Serializable]
        public struct InitData
        {
            public RectTransform Content;
            public RectTransform ScrollRect;
            public float Duration;
            public float Speed; // distance / time
        }

        public float Displacement;
        public float Distance;
        protected override void PostInit()
        {
            var currPos = initData.Content.anchoredPosition;
            currPos.y = 0;
            
            initData.Content.anchoredPosition = currPos;
            
            Displacement = RectExtensions.GetTargetYToMatchBottom(initData.Content.rect, initData.ScrollRect.rect);
            // throw new NotImplementedException();
            
            Distance = Mathf.Abs(Displacement);
            
            Duration = Distance / initData.Speed;
        }

        public float Duration;

        public override float T => ElapsedTime / Duration;

        public float TargetY;
        public override void Tick(float t)
        {
            TargetY = RectExtensions.GetTargetYToMatchBottom(initData.Content.rect, initData.ScrollRect.rect);

            var currPos = initData.Content.anchoredPosition;

            currPos.y = Mathf.Lerp(0f, TargetY, t);

            initData.Content.anchoredPosition = currPos;
            // throw new NotImplementedException();
        }
    }
}