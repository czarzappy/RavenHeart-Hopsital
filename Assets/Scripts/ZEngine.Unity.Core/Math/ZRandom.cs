using System;
using UnityEngine;
using Random = System.Random;

namespace ZEngine.Unity.Core.Math
{
    public static class ZRandom
    {
        private static Random Rand = new Random();
        
        public static T EnumValue<T>() where T : Enum
        {
            Type enumType = typeof(T);
            
            var values = Enum.GetNames(enumType);

            int len = values.Length;

            int idx = Rand.Next(0, len);
            
            return (T) Enum.Parse(enumType, values[idx]);
        }

        public static Vector2 Vector2()
        {
            // TODO: fix distribution
            Vector2 vector2 = new Vector2(Rand.Next(-1, 1), Rand.Next(-1, 1));

            return vector2.normalized;
        }
        
        public static T Item<T>(T[] items)
        {
            return items[Rand.Next(0, items.Length)];
        }
    }
}