using System;
using UnityEngine;

namespace ZEngine.Unity.Core.Attributes
{
    public class ActivationAttr : InitZAttr<ActivationAttr.InitData>
    {
        public struct InitData
        {
            /// <summary>
            /// In seconds
            /// </summary>
            public float ActivationDelay;
            
            // Action to perform on activation
            public Action ActivationAction;
        }
        
        private float mLastActivateTime;
        protected override void PostInit()
        {
            mLastActivateTime = Time.time;
        }
        
        public void Update()
        {
            if (Time.time - mLastActivateTime > initData.ActivationDelay)
            {
                mLastActivateTime = Time.time;

                initData.ActivationAction();
            }
        }
    }
}