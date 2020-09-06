using UnityEngine;

namespace ZEngine.Unity.Core.Extensions
{
    public static class TextureExt
    {
        public static Sprite ToSprite(this Texture2D tex2d)
        {
            var rect = new Rect(0, 0, tex2d.width, tex2d.height);
            var pivot = new Vector2(tex2d.width / 2f, tex2d.height / 2f);
            return Sprite.Create(tex2d, 
                rect, 
                pivot);
        }
    }
}