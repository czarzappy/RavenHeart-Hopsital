using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ZEngine.Unity.Core.Attributes
{
    public class ColorFadeZAttr : InitDurationTickZAttr<ColorFadeZAttr.InitData>
    {
        [Serializable]
        public struct InitData
        {
            public BlendType BlendType;
            public Gradient Gradient;
            
            public Color StartColor;
            public Color EndColor;
            
            public SourceType SourceType;
            public Material Material;
            public Image Image;
            public TMP_Text Text;
        }

        public enum BlendType
        {
            START_END,
            GRADIENT,
        }

        public enum SourceType
        {
            IMAGE,
            MATERIAL,
            TMP_TEXT,
        }
        
        protected override void PostInit()
        {
            Color startColor;
            switch (initData.BlendType)
            {
                case BlendType.GRADIENT:
                    startColor = initData.Gradient.Evaluate(0);
                    break;
                
                case BlendType.START_END:
                    startColor = initData.StartColor;
                    break;
                
                default:
                    throw new InvalidOperationException($"Attempting to init unknown blend type: {initData.BlendType}");
            }
            
            SetColor(startColor);
        }

        public override void Tick(float t)
        {
            Color color;

            switch (initData.BlendType)
            {
                case BlendType.GRADIENT:
                    color = initData.Gradient.Evaluate(t);
                    break;
                
                case BlendType.START_END:
                    color = Color.Lerp(initData.StartColor, initData.EndColor, t);
                    break;
                
                default:
                    throw new InvalidOperationException($"Attempting to tick unknown blend type: {initData.BlendType}");
            }

            SetColor(color);
        }
        

        private void SetColor(Color color)
        {
            switch (initData.SourceType)
            {
                case SourceType.IMAGE:
                    initData.Image.color = color;
                    break;
                case SourceType.MATERIAL:
                    initData.Material.color = color;
                    break;
                
                case SourceType.TMP_TEXT:
                    initData.Text.color = color;
                    break;
                
                default:
                    ZBug.Warn("ZATTR", $"Unknown source type: {initData.SourceType}");

                    break;
            }
        }
    }
}