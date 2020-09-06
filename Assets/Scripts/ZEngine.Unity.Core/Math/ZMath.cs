using System;
using UnityEngine;
using Random = System.Random;

namespace ZEngine.Unity.Core.Math
{
    public class ZMath
    {
        public static float Cos1To0(float f)
        {
            return (Mathf.Cos(f) + 1) / 2;
        }
        
        public static float Cos0To1(float f)
        {
            return (-Mathf.Cos(f) + 1) / 2;
        }
        
        public static float Sin0To1(float f)
        {
            return (Mathf.Sin(f) + 1) / 2;
        }
    }
}