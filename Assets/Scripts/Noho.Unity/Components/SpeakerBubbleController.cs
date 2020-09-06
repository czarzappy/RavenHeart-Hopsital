using Noho.Configs;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Models;

namespace Noho.Unity.Components
{
    public class SpeakerBubbleController : UIMonoBehaviour
    {
        public Image BGImage;
        public Image SpeakerImage;
        
        public void Init(CharacterConfig characterConfig)
        {
            BGImage.color = characterConfig.CharacterNameBackgroundColor.ToUnity();
            SpeakerImage.sprite = ZSource.Load<Sprite>(ResourcePath.Combine("Characters", characterConfig.DevName, characterConfig.FaceSpritePath));
        }
    }
}