using System;
using System.Collections.Generic;
using Noho.Models;
using Noho.Unity.Models;
using UnityEngine;
using ZEngine.Core.Dialogue.Models;
using ZEngine.Unity.Core.Attributes;
using ZEngine.Unity.Core.Extensions;

namespace Noho.Unity.Components
{
    public class StagePosController : MonoBehaviour
    {
        public CharacterController[] Characters;

        public int NumStagePositions
        {
            get
            {
                return Characters.Length;
            }
        }
        // public Image CharacterPrefab;
        
        public List<string> ActiveCharacters = new List<string>();
    
        // Start is called before the first frame update
        void Start()
        {
        }

        public void Init()
        {
            mActiveMovements = new List<FiniteTickZAttr>();
            int stagePosIdx = 0;
            foreach (CharacterController controller in Characters)
            {
                controller.ResetCharacter(stagePosIdx);
                controller.Hide();

                stagePosIdx++;
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void HandleMoves(List<CharacterMove> characterMoves)
        {
            // go through any movement and resolve it.
            foreach (FiniteTickZAttr finiteTickZAttr in mActiveMovements)
            {
                finiteTickZAttr.ForceEnd();
            }
            
            foreach (CharacterMove characterMove in characterMoves)
            {
                // bool isNew = true;
                // foreach (string activeCharacter in ActiveCharacters)
                // {
                //     if (characterMove.CharacterDevName == activeCharacter)
                //     {
                //         isNew = false;
                //         MoveExisting(characterMove);
                //         break;
                //     }
                // }
                //
                // if (isNew)
                // {
                // }
                
                MoveNew(characterMove);
            }
        }

        public RectTransform OffStageLeftMarker;
        public RectTransform OffStageRightMarker;

        private List<FiniteTickZAttr> mActiveMovements;

        public CharacterController GetCharactersByPos(int stagePos)
        {
            foreach (CharacterController characterController in Characters)
            {
                if (characterController.StagePosIndex == stagePos - 1)
                {
                    return characterController;
                }
            }

            ZBug.Warn("STAGEPOS", $"Attempted to get unknown stage pos: {stagePos}");
            return null;
        }

        public CharacterController GetCharacterByName(string characterDevName)
        {
            foreach (CharacterController characterController in Characters)
            {
                if (characterController.CharacterDevName == characterDevName)
                {
                    return characterController;
                }
            }
            
            return null;
        }

        public Vector2 GetClosestOffStagePosForStageIndex(int stagePosIndex)
        {
            float appearanceIndex = (stagePosIndex + 1f) / NumStagePositions;
            ZBug.Info("Character", $"appearanceIndex: {appearanceIndex}");
            return appearanceIndex < 0.5f ? OffStageLeftMarker.anchoredPosition : OffStageRightMarker.anchoredPosition;
        }
        
        private void MoveNew(CharacterMove characterMove)
        {
            // get where the character will be going to
            CharacterController controller = GetCharactersByPos(characterMove.NewStagePos);

            Vector2 startingPos = default;
            
            bool firstAppearance = true;

            // get where the character is at now, if they are on-screen
            CharacterController currentCharController = GetCharacterByName(characterMove.CharacterDevName);

            if (currentCharController != null)
            {
                ZBug.Info("STAGEPOS", $"Character is on-screen: {currentCharController}");
                startingPos = currentCharController.CharacterIdlePos;
                currentCharController.Hide();
                firstAppearance = false;
            }
            
            controller.Show();
            if (firstAppearance)
            {
                ZBug.Info("Character", $"[MoveNew] Adding image fade attribute to character: {characterMove.CharacterDevName}");
                controller.gameObject.InitFiniteTickZAttr<ImageFadeInZAttr, ImageFadeInZAttr.InitData>(new ImageFadeInZAttr.InitData
                {
                    Image = controller.CharacterImage,
                    Duration = NohoUISettings.CHARACTER_FADE_IN_DURATION,
                });

                startingPos = GetClosestOffStagePosForStageIndex(controller.StagePosIndex);
            }
            
            if (!controller.IsInit)
            {
                throw new InvalidOperationException("WTF - trying to move uninitializd character");
            }
            
            ZBug.Info("Character", $"[MoveNew] Adding translation attribute to character: {characterMove.CharacterDevName}");
            ZBug.Info("STAGEPOS", $"{controller} moving FROM {startingPos}");
            mActiveMovements.Add(controller.gameObject.InitDurationTickZAttr<TranslateUIZAttr, TranslateUIZAttr.InitData>(
            new TranslateUIZAttr.InitData
            {
                StartAnchoredPos = startingPos,
                EndAnchoredPos = controller.CharacterIdlePos,
            }, NohoUISettings.CHARACTER_WALK_IN_DURATION));

            controller.SetCharacter(characterMove.CharacterDevName);
            
            
            // CharacterNameContainer.Show();
            // CharacterImage.Show();
        }

        public CharacterController GetCharacter(string characterDevName)
        {
            string searchName = characterDevName;
            if (characterDevName == NohoParsingConstants.CHARACTER_DEV_NAME_EDGAR)
            {
                searchName = NohoParsingConstants.CHARACTER_DEV_NAME_ANNABEL;
            }
            
            foreach (CharacterController character in Characters)
            {
                if (character.CharacterDevName == searchName)
                {
                    return character;
                }
            }

            ZBug.Error("Character", $"No character with on screen with name: {characterDevName}, search name: {searchName}");
            return null;
        }

        public void HandleExpression(string characterDevName, string expressionType)
        {
            CharacterController controller = GetCharacter(characterDevName);

            if (controller == null)
            {
                ZBug.Warn("STAGEPOS", $"Unknown character {characterDevName}");
                return;
            }

            if (expressionType == "leave" || 
                expressionType == "leaves")
            {
                ZBug.Info("STAGEPOS",$"Character {characterDevName} is leaving!");
                
                var currentCharacter = GetCharacterByName(characterDevName);
                
                var dest = GetClosestOffStagePosForStageIndex(currentCharacter.StagePosIndex);
                
                ZBug.Info("STAGEPOS", $"{controller} moving TO {dest}");
                mActiveMovements.Add(controller.gameObject.InitDurationTickZAttr<TranslateUIZAttr, TranslateUIZAttr.InitData>(
                    new TranslateUIZAttr.InitData
                    {
                        StartAnchoredPos = controller.CharacterIdlePos,
                        EndAnchoredPos = dest,
                    }, NohoUISettings.CHARACTER_WALK_IN_DURATION));
                
                
                controller.SetCharacterDevName(null);
            }
            else
            {
                controller.SetExpression(expressionType);
            }
        }
    }
}
