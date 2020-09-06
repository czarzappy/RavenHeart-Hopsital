using System;
using System.IO;
using Noho.Configs;
using Noho.Models;
using Noho.Unity.Components;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;
using CharacterController = Noho.Unity.Components.CharacterController;

namespace Noho.Unity.Scenes
{
    public class PostOpMain : MonoBehaviour
    {
        public TMP_Text EpisodeTitle;
        public TMP_Text ProtagName;
        public TMP_Text RankTitle;
        public TMP_Text RankGrade;
        public TMP_Text TotalScore;
        public TMP_Text TotalTime;
        
        public Image SpeakerImage;
        public Image BGImage;
        public AudioSource BGM;
        public AudioClip LoopClip;

        public CharacterController CharacterController;

        public AnimationCurve ScaleCurve;

        public SpeakerBubbleController SpeakerBubble;
        
        public void Init()
        {
            ProtagName.text = PlayerPrefKeys.PlayerDisplayName;
            
            var score = BrainMain.Instance.Context.OperationManager.Score;

            var finalScore = score.FinalScore;
            TotalScore.text = $"{finalScore:N0}";

            var time = BrainMain.Instance.Context.OperationManager.ElapsedSeconds;

            string text = PrettyTime(time);
            TotalTime.text = text;
            
            string speakerDevName = BrainMain.Instance.Context.OperationManager.OperationDef.ToolTipSpeakerDevName;

            var speakerCharacterConfig = NohoConfigResolver.GetConfig<CharacterConfig>(speakerDevName);

            BGImage.color = speakerCharacterConfig.CharacterNameBackgroundColor.ToUnity();

            SpeakerImage.sprite = ZSource.Load<Sprite>(speakerCharacterConfig.DefaultSpritePath);
            
            SpeakerBubble.Init(speakerCharacterConfig);
            
            CharacterController.SetCharacter(NohoParsingConstants.CHARACTER_DEV_NAME_PROTAG);

            string episodeName = App.Instance.RuntimeData.PlayingEpisodeAsset.name;
            EpisodeTitle.text = $"{speakerCharacterConfig.RawName} - {episodeName}";
        }

        private string PrettyTime(float time)
        {
            int seconds = Mathf.FloorToInt(time);
            int minutes = seconds / 60;

            float pureSeconds = time - (minutes * 60);
            
            StringWriter sw = new StringWriter();

            sw.Write($"{minutes:D1}m {pureSeconds:N3}s");
            
            return sw.ToString();
        }


        public void OnDestroy()
        {
            UnInit();
        }
        
        public void UnInit()
        {
        }

        public bool FirstPlay = true;
        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Send.Msg(new PostOpCompleteMsg
                {
                    
                });
            }

            if (FirstPlay && !BGM.isPlaying)
            {
                BGM.clip = LoopClip;
                BGM.loop = true;
                BGM.Play();
                FirstPlay = false;
            }
        }
    }
}