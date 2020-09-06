using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core.Math;

namespace ZEngine.Unity.Core.Components
{
    public class RandomImage : AIDU
    {
        public Sprite[] Images;
        public Image Image;
        
        public override void Init()
        {
            Image.sprite = ZRandom.Item(Images);
        }

        public override void UnInit()
        {
        }
    }
}