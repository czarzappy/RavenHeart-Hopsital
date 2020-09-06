using Noho.Unity.Models;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZEngine.Unity.Core.Components;

namespace Noho.Unity.Scenes.Title
{
    public class ScenePanelController : UIMonoBehaviour
    {
        public Image PreviewImage;

        public TMP_Text TitleText;

        public Button PlayButton;


        public void Init(string titleText, Sprite previewSprite, int sceneIdx)
        {
            TitleText.text = titleText;

            PreviewImage.sprite = previewSprite;
            
            PlayButton.onClick.AddListener(() =>
            {
                App.Instance.RuntimeData.PlayingEpisodeSceneIdx = sceneIdx;
                ZBug.Info("TITLE", $"Scene index selected: {sceneIdx}");
                SceneManager.LoadSceneAsync("BrainScene", LoadSceneMode.Single);
            });
        }
    }
}