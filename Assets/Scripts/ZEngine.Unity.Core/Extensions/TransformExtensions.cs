using System.Collections.Generic;
using UnityEngine;

namespace ZEngine.Unity.Core.Extensions
{
    public static class TransformExtensions
    {
        public static void FlipX(this Transform transform)
        {
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        public static void SetToLastSibling(this Transform transform)
        {
            transform.SetSiblingIndex(transform.parent.childCount - 1);
        }

        public static void SetToFirstSibling(this Transform transform)
        {
            transform.SetSiblingIndex(0);
        }

        // public static void ScaleToChildren(this RectTransform rectTransform)
        // {
        //     Rect rect = rectTransform.rect;
        //     
        //     foreach (var child in rectTransform.GetChildren())
        //     {
        //         Rect childRect = child.rect;
        //
        //         childRect.center = rectTransform.anchoredPosition;
        //         
        //         rect = RectExtensions.Max(rect, childRect);
        //     }
        //
        //     rectTransform.sizeDelta = rect.size;
        // }

        public static void ScaleToChildren(this RectTransform rectTransform)
        {
            // Rect rect = rectTransform.rect;

            float sumHeight = 0f;
            // float maxWidth = 0f;
            foreach (var child in rectTransform.GetChildren())
            {
                Rect childRect = child.rect;

                sumHeight += childRect.height;
                // maxWidth = Mathf.Max(maxWidth, childRect.width);

                // childRect.center = rectTransform.anchoredPosition;

                // rect = RectExtensions.Max(rect, childRect);
            }

            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, sumHeight);
        }

        public static IEnumerable<Transform> GetChildren(this Transform transform)
        {
            int count = transform.childCount;

            for (int childIdx = 0; childIdx < count; childIdx++)
            {
                var child = transform.GetChild(childIdx);

                yield return child;
            }
        }

        public static IEnumerable<RectTransform> GetChildren(this RectTransform transform)
        {
            int count = transform.childCount;

            for (int childIdx = 0; childIdx < count; childIdx++)
            {
                var child = transform.GetChild(childIdx);

                yield return (RectTransform) child;
            }
        }
    }
}