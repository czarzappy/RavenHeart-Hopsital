using System.Collections;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using TMPro;
using UnityEngine;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Title
{
    public class AudioClipScrollContainer : ScrollContainer<AudioClipInfoScrollItem, AudioClip>
    {
        public AudioSource AudioSource;

        public TMP_Text LoadingText;
        public override IEnumerator Init()
        {
            MsgMgr.Instance.SubscribeTo<PlayAudioMsg>(OnPlayAudio);
            
            var items = ZSource.LoadAll<AudioClip>(NohoResourcePack.FOLDER_BGM);

            foreach (AudioClip item in items)
            {
                yield return AddNewItem(() => item);
            }

            LoadingText.text = "Loaded!";
        }

        public override void UnInit()
        {
        }

        private void OnPlayAudio(PlayAudioMsg message)
        {
            AudioSource.clip = message.AudioClip;
            AudioSource.Play();
        }
    }
}