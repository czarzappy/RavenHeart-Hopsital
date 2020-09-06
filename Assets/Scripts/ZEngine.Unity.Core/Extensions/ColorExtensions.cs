using UnityEngine;
using ZEngine.Core.Math;

namespace ZEngine.Unity.Core.Extensions
{
    public static class ColorExtensions
    {
        public static Color ToUnity(this ZColor color)
        {
            return new Color(color.r, color.g, color.b, color.a);
        }
        
        public static Color NoAlpha(this Color color)
        {
            return new Color(color.r, color.g, color.b, 0f);
        }
        
        public static Color FullAlpha(this Color color)
        {
            return new Color(color.r, color.g, color.b, 1f);
        }
        
        public static Color Alpha(this Color color, float a)
        {
            return new Color(color.r, color.g, color.b, a);
        }
        
        public static Color LowerAlpha(this Color color, float multiplier)
        {
            return new Color(color.r, color.g, color.b, color.a * multiplier);
        }
        
        public static Color Darken(this Color color, float fadeMulitplier)
        {
            return new Color(color.r * fadeMulitplier, 
                color.g * fadeMulitplier, 
                color.b * fadeMulitplier, 
                color.a);
        }
        
        public static Color Brighten(this Color color, float fadeMulitplier)
        {
            return new Color(color.r.Brighten(fadeMulitplier), 
                color.g.Brighten(fadeMulitplier), 
                color.b.Brighten(fadeMulitplier), 
                color.a);
        }

        public static float Brighten(this float f, float multiplier)
        {
            return Mathf.Lerp(f, 1, multiplier);
        }
    }
}