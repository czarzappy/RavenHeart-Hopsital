using System;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.UIElements;
using ZEngine.Unity.Core.Models;

namespace ZEngine.Unity.Core.Attributes
{
    public class ColorPaletteCycleZAttr : InitZAttr<ColorPaletteCycleZAttr.InitData>
    {
        [Serializable]
        public struct InitData
        {
            public ColorPalette Palette;
            public Image Image;
            public float CycleDuration;
        }

        private float mStartTime;
        protected override void PostInit()
        {
            mStartTime = Time.time;
        }

        public void Update()
        {
            float totalDuration = Time.time - mStartTime;
            
            int steps = Mathf.FloorToInt(totalDuration / initData.CycleDuration);

            float wholeDuration = steps * initData.CycleDuration;

            float partialDuration = totalDuration - wholeDuration;

            float t = partialDuration / initData.CycleDuration;

            int index = (steps) % initData.Palette.Colors.Length;
            int index2 = (index + 1) % initData.Palette.Colors.Length;

            Color start = initData.Palette.Colors[index];
            Color end = initData.Palette.Colors[index2];

            initData.Image.color = Color.Lerp(start, end, t);
        }
    }
}