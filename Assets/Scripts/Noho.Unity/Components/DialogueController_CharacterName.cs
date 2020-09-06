using Noho.Configs;
using Noho.Models;
using Noho.Unity.Models;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;
using ZEngine.Unity.Core.Attributes;
using ZEngine.Unity.Core.Extensions;

namespace Noho.Unity.Components
{
    public partial class DialogueController
    {
        public RectTransform CharacterNameContainer;
        public Image CharacterNameBG;
        
        public TMP_Text CharacterName;

        // public LocalizedString CharacterNameText;

        public void InitCharacterName()
        {
            CharacterNameContainer.Hide();
            // CharacterNameText.SetCharacterName("NARRATOR");
            // CharacterNameText.RegisterChangeHandler(OnCharacterNameChange);
        }

        private void OnCharacterNameChange(string value)
        {
            CharacterName.text = value;
        }

        private void SetSpeakerName(string characterDevName)
        {
            ZBug.Info("DIALOGUE", $"Setting speaker name: {characterDevName}");

            string displayName;
            switch (characterDevName)
            {
                case NohoParsingConstants.CHARACTER_DEV_NAME_PROTAG:
                    displayName = PlayerPrefKeys.PlayerDisplayName;
                    break;
                
                case NohoParsingConstants.CHARACTER_DEV_NAME_EDGAR:
                    displayName = "Edgar";
                    break;
                
                case NohoParsingConstants.CHARACTER_DEV_NAME_PATIENT:
                    displayName = "Patient";
                    break;
                
                default:
                    displayName = NohoConfigResolver.GetConfig<CharacterConfig>(characterDevName).RawName;
                    break;
            }

            CharacterName.text = displayName;
        }

        private void HandleNewCharacterName(string characterDevName, ref Color characterNameColor)
        {
            if (StagePosController == null)
            {
                return;
            }
            

            
            switch (characterDevName)
            {
                case NohoParsingConstants.CHARACTER_DEV_NAME_PATIENT:
                    break;
                default:
                    CharacterController character = StagePosController?.GetCharacter(characterDevName);

                    if (character == null)
                    {
                        ZBug.Info("DIALOGUE", $"Unknown character: {characterDevName}");
                        CharacterNameContainer.Hide();
                        return;
                    }
            
                    DEBUG_SpeakerController = character;
                    MoveCharacterNameUnderCharacter(character);
                    characterNameColor = character.CharacterConfig.CharacterNameBackgroundColor.ToUnity();
                    break;
            }

            CharacterNameBG.gameObject.InitDurationTickZAttr<ColorFadeZAttr, ColorFadeZAttr.InitData>(new ColorFadeZAttr.InitData
            {
                SourceType = ColorFadeZAttr.SourceType.IMAGE,
                Image = CharacterNameBG,
                
                BlendType = ColorFadeZAttr.BlendType.START_END,
                StartColor = CharacterNameBG.color.Alpha(.6f),
                EndColor = characterNameColor,
            }, NohoUISettings.CHARACTER_NAME_SHIFT_DURATION);
            
            SetSpeakerName(characterDevName);

            CharacterNameContainer.Show();
        }

        private void MoveCharacterNameUnderCharacter(CharacterController character)
        {
            float x = character.CharacterIdlePos.x;
            DEBUG_CharNameDestX = x;
                    
            Vector2 currentPos = CharacterNameContainer.anchoredPosition;
            DEBUG_CharNameStartX = currentPos.x;
                    
            Vector2 startPos = currentPos;
            Vector2 endPos = currentPos.TranslateToX(x);

            float dist = Vector3.Distance(startPos, endPos);
            float duration = dist / NohoUISettings.CHARACTER_NAME_SHIFT_SPEED;
            
            ZBug.Info("Dialogue", $"Character name distance: {dist}, duration: {duration}");
                    
            CharacterNameContainer.gameObject.InitDurationTickZAttr<TranslateUIZAttr, TranslateUIZAttr.InitData>(new TranslateUIZAttr.InitData
            {
                StartAnchoredPos = startPos,
                EndAnchoredPos = endPos,
            }, duration);
        }
    }
}