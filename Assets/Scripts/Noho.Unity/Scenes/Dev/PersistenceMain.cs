using System;
using UnityEngine.UI;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Math;
using ZEngine.Unity.Core.Scenes;

namespace Noho.Unity.Scenes.Dev
{
    public class PersistenceMain : UIMonoBehaviour
    {
        public Image Image;

        public void Start()
        {
            Image.rectTransform.anchoredPosition = ZRandom.Vector2() * 500;
        }
    }
}