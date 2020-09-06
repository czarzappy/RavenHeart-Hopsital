using System;
using UnityEngine;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Models;

namespace ZEngine.Unity.Core.Attributes
{
    public class UIPointerFollowZAttr : InitZAttr<UIPointerFollowZAttr.InitData>
    {
        public struct InitData
        {
            public AnchoredPos AnchorPosOffset;
            public Vector3 WorldPosOffset;

            public InitData(Vector2 anchorPosOffset = default, Vector3 worldPosOffset = default)
            {
                AnchorPosOffset = anchorPosOffset;
                WorldPosOffset = worldPosOffset;
            }
        }

        public Vector3 mouseWorld;
        public Vector3 thisWorld;
        public Vector2 viewportPosition;
        public Vector2 anchoredPos;
        public Vector3 anchoredPos3D;
        public AnchoredPos mouseCanvas;
        public AnchoredPos anchoredOffset;
        public Vector3 worldOffset;
        public Vector3 newPos;
        
        protected override void PostInit()
        {
            anchoredOffset = initData.AnchorPosOffset;
            worldOffset = initData.WorldPosOffset;
            // Vector3 mousePos = Input.mousePosition;

            // Vector3 mouseWorld = ZCamera.main.ScreenToWorldPoint(mousePos);
            // Vector3 startPos = transform.position;

            // mouseWorld.z = startPos.z;

            // MouseOffset = MouseOffseteWorld - startPos;
        }

        public void Update()
        {
            mouseWorld = ZInput.mouseWorldPosition;
            
            viewportPosition = ZCamera.main.WorldToViewportPoint(mouseWorld);
            mouseCanvas = rectTransform.WorldToCanvasSpace(mouseWorld);

            // rectTransform.anchoredPosition = mouseCanvas - initData.AnchorPosOffset;

            float currentWorldDepth = transform.position.z;
            anchoredPos = rectTransform.anchoredPosition;
            anchoredPos3D = rectTransform.anchoredPosition3D;

            newPos = mouseWorld - initData.WorldPosOffset;
            newPos.z = currentWorldDepth;

            thisWorld = newPos;
            transform.position = thisWorld;
        }
    }
}