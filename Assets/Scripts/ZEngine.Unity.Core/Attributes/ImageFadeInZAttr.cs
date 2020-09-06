using System;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core.Extensions;

namespace ZEngine.Unity.Core.Attributes
{
    public class ImageFadeInZAttr : InitFiniteTickZAttr<ImageFadeInZAttr.InitData>
    {
        [Serializable]
        public struct InitData
        {
            public Image Image;
            public float Duration;
        }

        private Color mStartColor;
        private Color mEndColor;
        
        protected override void PostInit()
        {
            if (initData.Image == null)
            {
                ZBug.Info("wtf");
            }
            Color color = initData.Image.color;
            mStartColor = color.NoAlpha();
            mEndColor = color.FullAlpha();
        }

        public override float T => ElapsedTime / initData.Duration;

        public override void Tick(float t)
        {
            initData.Image.color = Color.Lerp(mStartColor, mEndColor, t);
        }
    }
}