using System;
using UnityEngine;

namespace ZEngine.Unity.Core.Attributes
{
    public class MultiStopTranslateZAttr : InitFiniteTickZAttr<MultiStopTranslateZAttr.InitData>
    {
        [Serializable]
        public struct InitData
        {
            public Vector2[] Stops;
            public float Duration;
        }

        private int NumStops;
        private float StopDelta;
        
        protected override void PostInit()
        {
            rectTransform.anchoredPosition = initData.Stops[0];

            NumStops = initData.Stops.Length;

            StopDelta = initData.Duration / NumStops;
        }

        public override float T => ElapsedTime / initData.Duration;

        public override void Tick(float t)
        {
            float stopCompletion = Mathf.Clamp(t % StopDelta, 0 , StopDelta);
            
            int idx = (int) Mathf.Floor(t / StopDelta);
            
            int previousStopIdx = Mathf.Clamp(idx, 0, NumStops - 1);
            int nextStopIdx = Mathf.Min(previousStopIdx + 1, NumStops - 1);

            Vector3 prevStop = initData.Stops[previousStopIdx];
            Vector3 nextStop = initData.Stops[nextStopIdx];
            
            // Set our position as a fraction of the distance between the markers.
            rectTransform.anchoredPosition = Vector3.Lerp(prevStop, nextStop, stopCompletion);
        }
    }
}