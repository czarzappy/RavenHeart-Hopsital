using Noho.Unity.Messages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Title
{
    public class AudioClipInfoScrollItem : ScrollItem<AudioClip>
    {
        public TMP_Text Text;

        public Button CopyToClipboardBtn;
        public Button PlayBtn;

        private string mAssetName;

        private AudioClip mAudioClip; 
        public override void Init(AudioClip audioClip)
        {
            mAssetName = audioClip.name;
            mAudioClip = audioClip;

            Text.text = mAssetName;
            
            CopyToClipboardBtn.onClick.AddListener(OnCopyToClipboardClick);
            PlayBtn.onClick.AddListener(OnPlayClick);
        }

        private void OnPlayClick()
        {
            Send.Msg(new PlayAudioMsg
            {
                AudioClip = mAudioClip
            });
        }

        private void OnCopyToClipboardClick()
        {
            mAssetName.CopyToClipboard();
            Send.Msg(new NewNotifMsg
            {
                Notif = $"Copied \"{mAssetName}\" to clipboard!"
            });
        }
    }
}