using System;
using UnityEngine;

namespace ZEngine.Unity.Core.Attributes
{
    public class GravitateAttr : InitZAttr<GravitateAttr.InitData>
    {
        public struct InitData
        {
            public Transform GoalMarker;
            public float ForceCoeff;
            public float RangeOfInfluence;
        }

        protected override void PostInit()
        {
        }

        public float dist;
        public float force;

        public void Update()
        {
            var position = transform.position;
            position.z = 0;

            Vector3 goal = initData.GoalMarker.position;
            goal.z = 0;
            
            Vector3 direction = goal - position;

            Vector3 unitDir = direction.normalized;
            
            dist = direction.magnitude;

            if (initData.RangeOfInfluence > 0 && dist > initData.RangeOfInfluence)
            {
                return;
            }

            float ringDepth = initData.RangeOfInfluence - dist;

            force = initData.ForceCoeff * (1 / Mathf.Pow(dist, 3));

            // don't shoot past the goal
            force = Mathf.Min(dist, force);
            
            position += unitDir * force;
            transform.position = position;
        }
    }
}