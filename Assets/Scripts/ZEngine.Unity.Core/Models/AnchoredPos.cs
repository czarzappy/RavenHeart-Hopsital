using System;
using UnityEngine;

namespace ZEngine.Unity.Core.Models
{
    [Serializable]
    public struct AnchoredPos
    {
        public Vector2 Value;

        public static implicit operator AnchoredPos(Vector2 value)
        {
            return new AnchoredPos
            {
                Value = value
            };
        }
        
        public static implicit operator Vector2(AnchoredPos anchoredPos)
        {
            return anchoredPos.Value;
        }

        public static AnchoredPos operator -(AnchoredPos a, AnchoredPos b)
        {
            return a.Value - b.Value;
        }
    }
}