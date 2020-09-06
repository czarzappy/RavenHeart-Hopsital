using System;
using System.Collections.Generic;
using System.Linq;
using Noho.Configs;
using Noho.Extensions;
using Noho.Factories;
using Noho.Models;
using Noho.Parsing.Models;
using Noho.Unity.Factories;
using Noho.Unity.Models;
using UnityEditor;
using UnityEngine;
using ZEngine.Core.Dialogue.Models;
using ZEngine.Core.Dialogue.Parsers;
using ZEngine.Core.Utils;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Models;
using ZEngine.Unity.Editor.Core;
using ZEngine.Unity.Editor.Core.Validation;

namespace Noho.Unity.Editor.Menus
{
    public static class ScriptValidator
    {
        private const string PATH = "Assets/Noho/Validate Script(s)";

        [MenuItem(PATH, true)]
        public static bool ValidateScript_Validation()
        {
            return ZSelection.HasTexAssetsSelected();
        }
        
        [MenuItem(PATH)]
        public static void ValidateScript()
        {
            var textAssets = Selection.objects.Select((o => o as TextAsset));

            IScriptParser scriptParser = ScriptParserFactory.DefaultParser();

            foreach (TextAsset textAsset in textAssets)
            {
                string scriptName = textAsset.name;
                IEnumerable<object> objects;
                try
                {
                    var lines = LineUtil.GetLines(textAsset.text).AsParserLines();

                    objects = scriptParser.Deserialize(lines);
                }
                catch (Exception e)
                {
                    ZBug.Error("SCENE-VALIDATION", $"Error parsing {textAsset.name}, error: {e.Message}");
                    continue;
                }
                // int item = 0;

                int items = 0;
                foreach (object o in objects)
                {
                    ZBug.Info("SUMMARY", $"Script Item: {o}, type: {o.GetType()}, idx: {items}");
                    items++;
                    // ZBug.Info($"[Type: {o.GetType()}] IDX: {item++}");
                    // ZBug.Info(JsonConvert.SerializeObject(o, SETTINGS));

                    switch (o)
                    {
                        case CutSceneDef cutSceneDef:
                            ValidateCutscene(scriptName, cutSceneDef);
                            break;
                        
                        case OperationDef operationDef:

                            ValidateOperation(scriptName, operationDef);
                            break;
                        
                        default:
                            ZBug.Warn($"Unknown scene item type: {o}");
                            break;
                    }
                }
                
                ZBug.Info("SUMMARY", $"Number of script items: {items}");
            }
        }

        private static void ValidateCutscene(string scriptName, CutSceneDef cutSceneDef)
        {
            string loveFocusCharacter = null;
            foreach (var action in cutSceneDef.Actions)
            {
                switch (action.Type)
                {
                    case StageActionType.LOVE_FOCUS:
                        loveFocusCharacter = action.CharacterDevName;
                        break;
                    
                    case StageActionType.LOVE_MIN:
                    case StageActionType.LOVE_DELTA:
                        if (action.CharacterDevName == null && loveFocusCharacter == null)
                        {
                            ZBug.Error("SCENE-VALIDATION", $"No character for love defined! Action: {action}");
                        }
                        break;
                    
                    case StageActionType.BACKGROUND:
                        if (!ZSource.IsPresent<Sprite>(NohoResourcePack.FOLDER_BG, action.Asset))
                        {
                            ZBug.Error("SCENE-VALIDATION", $"Missing Sprite, path: {NohoResourcePack.FOLDER_BG}/{action.Asset}");
                        }
                        break;
                    
                    case StageActionType.BACKGROUND_MUSIC:
                        Validate.AudioClip(scriptName, new ResourcePath(NohoResourcePack.FOLDER_BGM, action.Asset));
                        break;
                    
                    case StageActionType.DIALOGUE:

                        bool skip = false;
                        switch (action.CharacterDevName)
                        {
                            case NohoParsingConstants.CHARACTER_DEV_NAME_PATIENT:
                            case NohoParsingConstants.CHARACTER_DEV_NAME_NARRATOR:
                            case NohoParsingConstants.CHARACTER_DEV_NAME_PROTAG:
                                skip = true;
                                break;
                        }

                        if (skip)
                        {
                            break;
                        }
                        
                        CharacterConfig character = NohoConfigResolver.GetConfig<CharacterConfig>(action.CharacterDevName);

                        if (character == null)
                        {
                            ZBug.Error("SCENE-VALIDATION", $"Missing Character config for character, name: {action.CharacterDevName}");
                        }
                        else
                        {
                            if (!ZSource.IsPresent<Sprite>(character.DefaultSpritePath))
                            {
                                ZBug.Error("SCENE-VALIDATION", $"Missing Sprite, character: {action.CharacterDevName}, path: {string.Join("/", character.DefaultSpritePath)}");
                            }
                        }
                        
                        break;
                }
            }
        }

        private static void ValidateOperation(string scriptName, OperationDef operationDef)
        {
            Validate.AudioClip(scriptName, new ResourcePath(NohoResourcePack.FOLDER_BGM, operationDef.BackgroundMusicPath));

            var initialIds = operationDef.InitialPatientIds;
            foreach (PatientDef patient in operationDef.Patients)
            {
                if (!initialIds.Contains(patient.Id))
                {
                    // Skipping patients that won't appear
                    break; 
                }
                
                SceneDef currentScene = null;
                foreach (var phaseDef in patient.PhaseDefs)
                {
                    if (phaseDef.SurgerySceneName != null)
                    {
                        SceneDef sceneDef = SceneDefFactory.FromResources(phaseDef.SurgerySceneName);
                        // foreach (var buildScene in buildScenes)
                        // {
                        //     if (buildScene.path == surgeryScenePath)
                        //     {
                        //         found = true;
                        //         break;
                        //     }
                        // }

                        if (sceneDef == null)
                        {
                            ZBug.Error("SCENE-VALIDATION", $"[Script: {scriptName}]Missing build scene: {phaseDef.SurgerySceneName}");
                            continue;
                        }

                        currentScene = sceneDef;
                    }

                    // Can't validate gunk is scene doesn't exist
                    if (currentScene == null)
                    {
                        continue;
                    }

                    foreach (StageAction stageAction in phaseDef.Actions)
                    {
                        switch (stageAction.Type)
                        {
                            case StageActionType.GUNK_INIT:
                            {
                                string gunkTag = stageAction.GunkType;
                                int numInScene = currentScene.NumEntitiesWithTag(gunkTag);

                                int expected = stageAction.GunkAmount;
                                if (numInScene < expected)
                                {
                                    ZBug.Error("SCENE-VALIDATION", $"[Script: {scriptName}][{currentScene.Name}] Not enough gunk [{gunkTag}], expected: {expected}, number in scene: {numInScene}");
                                }
                                break;
                            }
                            case StageActionType.GUNK_INDICES:
                            {
                                string gunkTag = stageAction.GunkType;
                                int numInScene = currentScene.NumEntitiesWithTag(gunkTag);

                                foreach (int gunkIndex in stageAction.CleanGunkIndices())
                                {
                                    // gunkIndex is 1-based
                                    if (numInScene < gunkIndex)
                                    {
                                        ZBug.Error("SCENE-VALIDATION", $"[Script: {scriptName}][{currentScene.Name}] Index gunk [{gunkTag}] out of bound, expected idx: {gunkIndex}, number in scene: {numInScene}");
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}