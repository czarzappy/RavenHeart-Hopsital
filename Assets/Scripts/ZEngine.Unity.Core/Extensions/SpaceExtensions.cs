using UnityEngine;
using ZEngine.Unity.Core.Models;

namespace ZEngine.Unity.Core.Extensions
{
    public static class SpaceExtensions
    {
        public static AnchoredPos WorldToCanvasSpace(this RectTransform canvasRect, Vector3 worldPos)
        {
            Vector2 viewportPosition = ZCamera.main.WorldToViewportPoint(worldPos);

            var sizeDelta = canvasRect.sizeDelta;
            
            AnchoredPos worldObjectScreenPosition = (viewportPosition * sizeDelta) - (sizeDelta * 0.5f);

            // AnchoredPos worldObjectScreenPosition = viewportPosition;
            // AnchoredPos worldObjectScreenPosition = new Vector2(
            //     ((viewportPosition.x * sizeDelta.x) - (sizeDelta.x * 0.5f)),
            //     ((viewportPosition.y * sizeDelta.y) - (sizeDelta.y * 0.5f)));

            return worldObjectScreenPosition;
        }
    }
}