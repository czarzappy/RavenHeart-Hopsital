using Noho.Configs;
using Noho.Models;
using UnityEngine;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Components;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace Noho.Unity.Scenes.Title
{
    public class CharacterPanelController : UIMonoBehaviour
    {
        public Button SelectButton;
        public NohoConstants.ProtagBaseModel ModelType;

        public Image CharacterImage;

        public void Start()
        {
            Init();
        }

        private void Init()
        {
            SelectButton.onClick.AddListener(OnSelectClicked);

            var config = NohoConfigResolver.GetConfig<CharacterConfig>($"PROTAG-{ModelType}");
            
            CharacterImage.sprite = ZSource.Load<Sprite>(config.DefaultSpritePath);
        }

        private void OnSelectClicked()
        {
            PlayerPrefs.SetInt(PlayerPrefKeys.PLAYER_MODEL_TYPE, (int) ModelType);
            PlayerPrefs.Save();
        }
    }
}