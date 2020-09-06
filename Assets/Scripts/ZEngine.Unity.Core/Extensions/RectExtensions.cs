using UnityEngine;

namespace ZEngine.Unity.Core.Extensions
{
    public static class RectExtensions
    {
        public static Rect Max(Rect a, Rect b)
        {
            Rect rect = new Rect
            {
                xMin = Mathf.Min(a.xMin, b.xMin),
                yMin = Mathf.Min(a.yMin, b.yMin),
                xMax = Mathf.Max(a.xMax, b.xMax),
                yMax = Mathf.Max(a.yMax, b.yMax)
            };
            
            return rect;
        }

        public static float GetTargetYToMatchBottom(Rect content, Rect view)
        {
            float contentHeight = content.height;
            float viewHeight = view.height;

            float targetY = contentHeight - viewHeight;
            return targetY;
        }
    }
}