using System.Collections;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using Noho.Unity.Scenes.Surgery;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;
using ZEngine.Unity.Core.Models;
using ZEngine.Unity.Core.Mods;

namespace Noho.Unity.Scenes.Title
{
    public class EpisodeSelectController : ScrollContainer<EpisodePanelController, (string, ResourcePath, TextAsset)>
    {
        public Button BackButton;

        public override IEnumerator Init()
        {
            MsgMgr.Instance.SubscribeTo<LoadCharacterEpisodesMsg>(OnLoadCharacterEpisodes);
            BackButton.onClick.AddListener(OnBackButtonClick);
            
            yield break;
        }

        private void OnBackButtonClick()
        {
            Send.Msg(new CloseWinMsg());
        }

        public override void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<LoadCharacterEpisodesMsg>(OnLoadCharacterEpisodes);
        }

        private void OnLoadCharacterEpisodes(LoadCharacterEpisodesMsg msg)
        {
            var path = new ResourcePath(NohoResourcePack.FOLDER_SCRIPTS, msg.CharacterDevName);
            var episodes = ZSource.LoadAll<TextAsset>(path);

            ClearChildren();
            foreach (TextAsset textAsset in episodes)
            {
                ZBug.Info("EPISODE", $"Showing episode: {textAsset.name}");
                StartCoroutine(AddNewItem(() => (textAsset.name, new ResourcePath(), textAsset)));
            }
        }
    }
}