using System.Collections;
using Noho.Unity.Models;
using TMPro;
using UnityEngine;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Components;

namespace Noho.Unity.Scenes.Title
{
    public class BackgroundScrollContainer : ScrollContainer<SpriteInfoScrollItem, Sprite>
    {
        public TMP_Text LoadingText;
        public override IEnumerator Init()
        {
            var sprites = ZSource.LoadAll<Sprite>(NohoResourcePack.FOLDER_BG);

            foreach (Sprite sprite in sprites)
            {
                yield return AddNewItem(() => sprite);
            }

            LoadingText.text = "Loaded!";
        }

        public override void UnInit()
        {
        }
    }
}