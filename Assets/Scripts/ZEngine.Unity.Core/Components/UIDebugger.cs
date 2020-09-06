using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZEngine.Unity.Core.Components
{
    public class UIDebugger : UIMonoBehaviour
    {
        public Vector2 Pivot;
        public Vector2 AnchoredPos;
        public Vector2 AnchoredMin;
        public Vector2 AnchoredMax;

        public List<Vector2> Sizes;
        public List<Vector2> Centers;
        public List<Vector2> Mins;
        public List<Vector2> Maxs;
        
        public Vector2 GlobalAnchoredPos;
        
        public int Depth;
        public void Update()
        {
            var anchoredPosition = rectTransform.anchoredPosition;
            Pivot = rectTransform.pivot;
            
            AnchoredPos = anchoredPosition;
            AnchoredMin = rectTransform.anchorMin;
            AnchoredMax = rectTransform.anchorMax;

            Sizes.Clear();
            Centers.Clear();
            Mins.Clear();
            Maxs.Clear();
            
            
            RectTransform trans = (RectTransform) this.rectTransform;
            var rect = trans.rect;
            Vector2 size = new Vector2(rect.width, rect.height);
            Sizes.Add(size);

            GlobalAnchoredPos = anchoredPosition;

            Vector2 nextPivot = Pivot;
            Vector2 nextAnchorMin = AnchoredMin;
            Vector2 nextAnchorMax = AnchoredMax;
            
            
            Depth = 0;
            while ((trans = (RectTransform) trans.parent) != null)
            {
                GlobalAnchoredPos += trans.anchoredPosition;

                rect = trans.rect;
                size = new Vector2(rect.width, rect.height);
                Sizes.Add(size);

                Vector2 center = new Vector2(size.x * nextPivot.x, size.y * nextPivot.y);
                Centers.Add(center);
                
                Vector2 min = new Vector2(size.x * nextAnchorMin.x, size.y * nextAnchorMin.y);
                Mins.Add(min);
                
                Vector2 max = new Vector2(size.x * nextAnchorMax.x, size.y * nextAnchorMax.y);
                Maxs.Add(max);
                
                
                
                nextPivot = trans.pivot;
                nextAnchorMin = trans.anchorMin;
                nextAnchorMax = trans.anchorMax;
                
                Depth++;
            }
        }
    }
}