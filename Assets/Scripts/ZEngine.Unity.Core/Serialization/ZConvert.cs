using UnityEditor;
using UnityEngine;

namespace ZEngine.Unity.Core.Serialization
{
    public static class ZConvert
    {
        static ZConvert()
        {
            
        }


        public static string ToJson(object obj)
        {
            return JsonUtility.ToJson(obj, true);
        }

        public static T FromJson<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
    }
}