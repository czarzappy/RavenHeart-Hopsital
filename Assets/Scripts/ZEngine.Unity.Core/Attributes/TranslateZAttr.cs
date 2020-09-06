using System;
using UnityEngine;

namespace ZEngine.Unity.Core.Attributes
{
    public class TranslateZAttr : InitDurationTickZAttr<TranslateZAttr.InitData>
    {
        [Serializable]
        public struct InitData
        {
            public Vector3 StartPos;
            public Vector3 EndPos;
            public Action OnEndCallback;
        }
        
        // Movement speed in units per second.
        // private readonly float Speed = 10.0F;
        
        private float mTotalDisplacement;

        protected override void PostInit()
        {
            // Calculate the journey length.
            mTotalDisplacement = Vector3.Distance(initData.StartPos, initData.EndPos);

            gameObject.transform.position = initData.StartPos;

            OnEnd = initData.OnEndCallback;
        }

        public override void Tick(float t)
        {
            // Set our position as a fraction of the distance between the markers.
            transform.position = Vector3.Lerp(initData.StartPos, initData.EndPos, t);
        }
    }
}