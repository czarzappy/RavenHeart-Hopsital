using Noho.Unity.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Models;

namespace Noho.Unity.Scenes.Title
{
    public class EpisodePanelController : ScrollItem<(string, ResourcePath, TextAsset)>
    {
        public Image PreviewImage;

        public TMP_Text TitleText;

        public Button PlayButton;


        public override void Init((string, ResourcePath, TextAsset) initData)
        {
            (string titleText, ResourcePath previewSpritePath, TextAsset episode) = initData;
            
            TitleText.text = titleText;
            
            // use default image if none
            if (previewSpritePath.Path != null)
            {
                PreviewImage.sprite = ZSource.Load<Sprite>(previewSpritePath);
            }
            
            PlayButton.onClick.AddListener(() =>
            {
                ZBug.Info("TITLE", $"Episode selected: {episode.name}");
                // App.Instance.RuntimeData.PlayingEpisodeAsset = episode;
                
                // Send.Msg(new ShowWinMsg
                // {
                //     Win = Win.SCENE_SELECT
                // });
                
                // AppManager.Instance.UnlockNextEpisode(item.character.DevName);
                //     
                // var episode = item.Item2;

                AppManager.Instance.StartEpisode(episode);
            });
        }
    }
}