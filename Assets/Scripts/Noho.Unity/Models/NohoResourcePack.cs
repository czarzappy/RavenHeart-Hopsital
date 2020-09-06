using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.IO;
using ZEngine.Unity.Core.Models;
using ZEngine.Unity.Core.Mods;

namespace Noho.Unity.Models
{
    public class NohoResourcePack : ResourcePack
    {
        public const string FOLDER_CHAR = "Characters";
        public const string FOLDER_BG = "BG";
        public const string FOLDER_SCRIPTS = "Scripts";
        public const string SCRIPT_FILE_PATTERN = "*.txt";
        public const string FOLDER_SFX = "SFX";
        public const string FOLDER_BGM = "BGM";
        public const string FOLDER_SURGERIES = "Surgeries";
        
        public string AbsoluteBaseDirectoryPath;

        public IEnumerable<string> BGs
        {
            get
            {
                string bgsPath = Path.Combine(AbsoluteBaseDirectoryPath, FOLDER_BG);

                return ZDir.GetFiles(bgsPath);
            }
        }

        public IEnumerable<string> BGMs
        {
            get
            {
                string bgmsPath = Path.Combine(AbsoluteBaseDirectoryPath, FOLDER_BGM);
                
                return ZDir.GetFiles(bgmsPath);
            }
        }

        public IEnumerable<string> CharacterDirs
        {
            get
            {
                string charsPath = Path.Combine(AbsoluteBaseDirectoryPath, FOLDER_CHAR);
                
                return ZDir.GetDirs(charsPath);
            }
        }

        public IEnumerable<string> ScriptDirs
        {
            get
            {
                string scriptsPath = Path.Combine(AbsoluteBaseDirectoryPath, FOLDER_SCRIPTS);
                
                return ZDir.GetDirs(scriptsPath);
            }
        }

        public IEnumerable<string> CharacterScriptDirs(string characterDevName)
        {
            string scripts = Path.Combine(AbsoluteBaseDirectoryPath, FOLDER_SCRIPTS, characterDevName);

            return ZDir.GetFiles(scripts, SCRIPT_FILE_PATTERN);
        }

        public static NohoResourcePack Load(string resourcePackFolder)
        {
            NohoResourcePack nohoResourcePack = new NohoResourcePack
            {
                AbsoluteBaseDirectoryPath = resourcePackFolder
            };

            return nohoResourcePack;
        }
        

        private string LoadCharacterConfig(string characterDir)
        {
            string filePath = Path.Combine(AbsoluteBaseDirectoryPath, FOLDER_CHAR, characterDir, "config.json");

            return File.ReadAllText(filePath);
        }


        private TextAsset LoadCharacterScript(string characterDevName, string filename)
        {
            string path = Path.Combine(AbsoluteBaseDirectoryPath, FOLDER_SCRIPTS, characterDevName, $"{filename}.txt");

            return ZFile.LoadTextAsset(path);
        }

        public Texture2D LoadBackgroundAsTex2D(string backgroundName)
        {
            string filePath = Path.Combine(AbsoluteBaseDirectoryPath, FOLDER_BG, $"{backgroundName}.png");

            if (!File.Exists(filePath))
            {
                ZBug.Warn("RESOURCE-PACK", $"File does not exist, filePath: {filePath}");
                return null;
            }

            return ZFile.LoadPNG(filePath);
        }

        public Sprite LoadBackgroundAsSprite(string backgroundName)
        {
            var tex2d = LoadBackgroundAsTex2D(backgroundName);

            if (tex2d == null)
            {
                ZBug.Warn("RESOURCE-PACK", $"Unknown background: {backgroundName}");
                return null;
            }
            
            return tex2d.ToSprite();
        }

        private AudioClip LoadBackgroundMusicAsAudioClip(string audioFileName)
        {
            string filePath = Path.Combine(AbsoluteBaseDirectoryPath, FOLDER_BGM, $"{audioFileName}.ogg");

            if (!File.Exists(filePath))
            {
                ZBug.Warn("RESOURCE-PACK", $"File does not exist, filePath: {filePath}");
                return null;
            }

            return ZFile.LoadAudioClip(filePath);
        }
        
        // public string GetScripts
        public override bool IsPresent<T>(ResourcePath resourcePath)
        {
            string firstPart = resourcePath[0];

            Type type = typeof(T);

            switch (firstPart)
            {
                case FOLDER_CHAR:
                    break;
                
                case FOLDER_BG:
                    if (type != typeof(Sprite))
                    {
                        return false;
                    }
                    break;
                
                case FOLDER_SFX:
                case FOLDER_BGM:
                    if (type != typeof(AudioClip))
                    {
                        return false;
                    }
                    break;
                
                case FOLDER_SCRIPTS:
                    break;
                
                case FOLDER_SURGERIES:
                    // not long enough
                    if (resourcePath.Length == 1)
                    {
                        ZBug.Warn("RESOURCE-PACK", "Surgeries");
                        return false;
                    }
                    
                    break;
                
                default:
                    return false;
            }

            return false;
        }

        public override T Load<T>(ResourcePath resourcePath)
        {
            string firstPart = resourcePath[0];
            Type type = typeof(T);

            switch (firstPart)
            {
                case FOLDER_SCRIPTS:
                    if (type == typeof(TextAsset))
                    {
                        if (resourcePath.Length == 3)
                        {
                            string characterDevName = resourcePath[1];
                            string filename = resourcePath[2];
                            // string path = Path.Combine(AbsoluteBaseDirectoryPath, resourcePath[0], resourcePath[1], $"{filename}.txt");
                            //
                            // if (!File.Exists(path))
                            // {
                            //     return null;
                            // }
                            //
                            // string text = File.ReadAllText(path);
                            // TextAsset textAsset = new TextAsset(text)
                            // {
                            //     name = filename,
                            // };
                            //
                            // return textAsset as T;

                            return LoadCharacterScript(characterDevName, filename) as T;
                        }
                    }
                    
                    break;
                
                case FOLDER_CHAR:

                    if (type == typeof(Sprite))
                    {
                        // string filenmae = resourcePath[2];

                        string path = Path.Combine(resourcePath.Path);

                        path = Path.Combine(AbsoluteBaseDirectoryPath, path) + ".png";

                        var tex2d = ZFile.LoadPNG(path);

                        if (tex2d == null)
                        {
                            ZBug.Warn("RESOURCE-PACK", $"Unknown sprite path: {path}");
                            return null;
                        }
                        
                        return tex2d.ToSprite() as T;
                    }
                    
                    break;
                
                case FOLDER_BG:
                    if (type != typeof(Sprite))
                    {
                        return null;
                    }

                    return LoadBackgroundAsSprite(resourcePath[1]) as T;
                
                case FOLDER_SFX:
                case FOLDER_BGM:
                    if (type != typeof(AudioClip))
                    {
                        return null;
                    }
                    
                    return LoadBackgroundMusicAsAudioClip(resourcePath[1]) as T;
                
                case FOLDER_SURGERIES:
                    // not long enough
                    if (resourcePath.Length == 1)
                    {
                        ZBug.Warn("RESOURCE-PACK", "Surgeries");
                        return null;
                    }
                    
                    break;
                
                default:
                    ZBug.Warn("RESOURCE-PACK", $"Unhandled path: {firstPart}");
                    return null;
            }

            return null;
        }

        public override IEnumerable<T> LoadAll<T>(ResourcePath resourcePath)
        {
            string firstPart = resourcePath[0];
            Type type = typeof(T);

            switch (firstPart)
            {
                case FOLDER_SCRIPTS:
                    if (type == typeof(TextAsset))
                    {
                        if (resourcePath.Length == 2)
                        {
                            if (resourcePath[0] == FOLDER_SCRIPTS)
                            {
                                string characterDevName = resourcePath[1];

                                foreach (string characterScriptFile in CharacterScriptDirs(characterDevName))
                                {
                                    ZBug.Info("RESOURCE-PACK", $"LoadAll, {characterScriptFile}");
                                    yield return ZFile.LoadTextAsset(characterScriptFile) as T;
                                }
                            }
                        }
                    }
                    
                    break;
                
                case FOLDER_CHAR:
                    if (type == typeof(TextAsset))
                    {
                        foreach (string characterDir in CharacterDirs)
                        {
                            // string text = ;
                            string test = LoadCharacterConfig(characterDir);

                            TextAsset textAsset = new TextAsset(test)
                            {
                                name = "config",
                            };


                            yield return textAsset as T;
                            // TextAsset.Create
                        }
                    }
                    
                    break;
                
                case FOLDER_BG:
                    if (type != typeof(Sprite))
                    {
                        yield break;
                    }

                    // return LoadBackgroundAsSprite(resourcePath[1]) as T;
                    yield break;
                
                case FOLDER_SFX:
                case FOLDER_BGM:
                    if (type != typeof(AudioClip))
                    {
                        yield break;
                    }
                    break;
                
                case FOLDER_SURGERIES:
                    // not long enough
                    if (resourcePath.Length == 1)
                    {
                        ZBug.Warn("RESOURCE-PACK", "Surgeries");
                        yield break;
                    }
                    
                    break;
                
                default:
                    yield break;
            }

            yield break;
        }
    }
}