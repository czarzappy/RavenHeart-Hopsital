using System;
using UnityEngine;

namespace ZEngine.Unity.Core.Attributes
{
    public class DistanceBasedProportionAttribute : MonoBehaviour
    {
        public Transform EndMarker;

        public float ScaleCoeff;

        public float MinScalar;

        private Vector3 mInitialScale;
        public void Start()
        {
            mInitialScale = transform.localScale;
        }

        public void Update()
        {
            var position = transform.position;
            
            Vector3 direction = EndMarker.position - position;
            
            float dist = direction.magnitude;

            float force = ScaleCoeff * (1 / Mathf.Pow(dist, 3f));

            float scalar = 1 / force;
            // force = Mathf.Clamp(force, 0, 1);

            scalar = Mathf.Clamp(scalar, MinScalar, 1);
            
            transform.localScale = mInitialScale * scalar;
        }
    }
}