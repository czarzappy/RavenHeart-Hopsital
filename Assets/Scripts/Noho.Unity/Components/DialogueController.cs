using System.Collections.Generic;
using Noho.Models;
using Noho.Parsing.Models;
using Noho.Unity.Messages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Components
{
    public partial class DialogueController : MonoBehaviour
    {
        public Image DialogueBackground;
        public TMP_Text DialogueText;
        public Color DefaultBGColor;
        public Color NarratorBGColor;
        public StagePosController StagePosController;
        public ChoiceContainerController ChoiceContainerController;

        #region Debug

        public string DEBUG_Speaker;
        public CharacterController DEBUG_SpeakerController;
        public float DEBUG_CharNameDestX;
        public float DEBUG_CharNameStartX;

        #endregion
        public string NextText;
        public float Hertz;
        
        private string mLastCharacterDevName;

        public void Init(StagePosController stagePosController)
        {
            StagePosController = stagePosController;
            
            DialogueBackground.Disable();

            DialogueText.Hide();
            ChoiceContainerController.Hide();

            InitCharacterName();
        }

        public void UnInit()
        {
            if (ChoiceContainerController != null)
            {
                ChoiceContainerController.Hide();
            }
            
            // Send.Msg();
        }
        
        public void HandleDialogue(string characterDevName, string dialogueText)
        {
            ZBug.Info("Dialogue", "Handling Dialogue");
            Color characterNameColor = Color.magenta;
            DEBUG_Speaker = characterDevName;
            Color bgColor = DefaultBGColor;
            switch (characterDevName)
            {
                case NohoParsingConstants.CHARACTER_DEV_NAME_NARRATOR:
                // case NohoParsingConstants.CHARACTER_DEV_NAME_PATIENT:
                    bgColor = NarratorBGColor;

                    characterNameColor = bgColor;
                    CharacterNameContainer.Hide();
                    break;
                
                default:
                    HandleNewCharacterName(characterDevName, ref characterNameColor);
                    break;
            }
            NextText = dialogueText.SanitizeProtag();

            int textScrollRate = PlayerPrefs.GetInt(PlayerPrefKeys.TEXT_SCROLL_RATE, (int) PlayerPrefController.EnumOption.NORMAL);
            
            ZBug.Info("Dialogue", $"Current scroll, key: [{PlayerPrefKeys.TEXT_SCROLL_RATE}], rate: {textScrollRate}");
            switch (textScrollRate)
            {
                case -1:
                case 0:
                    ZBug.Info("Dialogue", "Instant text scrolling");
                    DialogueText.text = NextText;
                    break;
                
                default:
                    ZBug.Info("Dialogue", "Setting next dialogue");
                    NextTextIndex = 0;

                    // float textScrollRate = PlayerPrefs.GetInt(PlayerPrefKeys.TEXT_SCROLL_RATE, 60);
                    ZBug.Info("Dialogue",$"Invoking repeating text tick, rate: {textScrollRate}");

                    CancelInvoke(nameof(TextTick));
                    InvokeRepeating(nameof(TextTick), 0f, 1f / textScrollRate);
                    break;
            }

            
            DialogueBackground.color = bgColor;
            DialogueBackground.Enable();
            DialogueText.Show();
            ChoiceContainerController.Hide();
            
            Send.Msg(new DialogueDisplayedMsg
            {
                Speaker = characterDevName,
                Dialogue = dialogueText,
                Color = characterNameColor,
            });
        }

        public int NextTextIndex;
        public void TextTick()
        {
            ZBug.Info("TextTick","Going");
            NextTextIndex++;
            if (NextTextIndex > NextText.Length)
            {
                ZBug.Info("Dialogue","Cancelling text tick");
                CancelInvoke(nameof(TextTick));
                return;
            }
            
            string newText = NextText.Substring(0, NextTextIndex);
            DialogueText.text = newText;
        }

        public void HandleChoices(List<StageBranchDef> stageActionChoices, int numberOfChoicesToSelect)
        {
            ChoiceContainerController.Show();
            ZBug.Info("Dialogue", "Handling choices");
            ChoiceContainerController.Load(stageActionChoices, numberOfChoicesToSelect);
        }
    }
}