using UnityEngine;

namespace ZEngine.Unity.Core.Extensions
{
    public static class VectorExt
    {
        public static Vector3[] QuadVertsToPolyPoints(this Vector3[] arr)
        {
            return new[]
            {
                arr[0],
                arr[1],
                arr[3],
                arr[2],
            };
        }
        
        public static Vector2[] ToVec2(this Vector3[] array)
        {
            Vector2[] result = new Vector2[array.Length];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = array[i];
            }

            return result;
        }
        
        public static Vector3 TranslateToX(this Vector3 vector3, float x)
        {
            Vector3 newVector3 = vector3;
            newVector3.x = x;
            return newVector3;
        }
        
        public static Vector2 TranslateToX(this Vector2 vector2, float x)
        {
            Vector3 newVector2 = vector2;
            newVector2.x = x;
            return newVector2;
        }

        public static Vector3 GetNormalDirection(Vector3 start, Vector3 end)
        {
            Vector3 dir = (end - start);
            dir.Normalize();

            return dir;
        }

        public static Vector3 GetLeft(this Vector3 vector3)
        {
            return new Vector3(-vector3.y, vector3.x, vector3.z);
        }

        public static Vector3 GetRight(this Vector3 vector3)
        {
            return new Vector3(vector3.y, -vector3.x, vector3.z);
        }

        public static Vector3 ZeroZ(this Vector3 vector3)
        {
            return new Vector3(vector3.x, vector3.y, 0);
        }
    }
}