using System;
using UnityEngine;

namespace ZEngine.Unity.Core.Models
{
    [Serializable]
    public class PoolSettings
    {
        public ResourcePath PrefabResourcePath;

        public MaxOverflowSettings Overflow;

        public bool HasMax
        {
            get
            {
                if (Overflow == null)
                {
                    return false;
                }

                return true;
            }
        }

        public Transform ParentTransform;
    }

    [Serializable]
    public class MaxOverflowSettings
    {
        public int SoftCap;
        public int HardCap;
    }
}