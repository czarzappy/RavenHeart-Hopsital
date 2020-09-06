using System.Collections.Generic;
using UnityEngine;
using ZEngine.Core.Localization;
using ZEngine.Core.Math;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Serialization;

namespace Noho.Configs.Factories
{
    public static class CharacterConfigFactory
    {
        public static List<CharacterConfig> GenerateCharacters(LocIdResolver locIdResolver)
        {
            List<CharacterConfig> characterConfigs = new List<CharacterConfig>();

            // TODO: delete
            #region TC
            //
            // characterConfigs.Add(new CharacterConfig
            // {
            //     DevName = "DEREK",
            //     // //Name = locIdResolver
            //     CharacterNameBackgroundColor = ZColor.blue.Brighten(.5f),
            //     DefaultPoseDevName = "DEFAULT",
            //     Poses = new List<CharacterPose>
            //     {
            //         new CharacterPose
            //         {
            //             ExpressionDevName = "DEFAULT",
            //             SpritePath = new []{"Characters", "PROTAG"}
            //         }
            //     }
            // });
            //
            // characterConfigs.Add(new CharacterConfig
            // {
            //     DevName = "KASAL",
            //     CharacterNameBackgroundColor = ZColor.red.Brighten(.5f),
            //     
            //     DefaultPoseDevName = "DEFAULT",
            //     Poses = new List<CharacterPose>
            //     {
            //         new CharacterPose
            //         {
            //             ExpressionDevName = "DEFAULT",
            //             SpritePath = new []{"Characters", "HOLIDAY"}
            //         },
            //     }
            // });
            //
            // characterConfigs.Add(new CharacterConfig
            // {
            //     DevName = "MARY",
            //     CharacterNameBackgroundColor = ZColor.magenta.Brighten(.5f),
            //     DefaultPoseDevName = "DEFAULT",
            //     Poses = new List<CharacterPose>
            //     {
            //         new CharacterPose
            //         {
            //             ExpressionDevName = "DEFAULT",
            //             SpritePath = new []{"Characters", "MARY"}
            //         },
            //         new CharacterPose
            //         {
            //             ExpressionDevName = "angry",
            //             SpritePath = new []{"Characters", "ANNABEL"}
            //         },
            //     }
            // });
            //
            #endregion

            int id = 0;
            #region Noho
            
            #region Protag
            
            characterConfigs.Add(new CharacterConfig
            {
                Id = id++,
                DevName = "PROTAG",
                ////Name = locIdResolver.Register("You"),
                Size = CharacterSize.MEDIUM,
                // //Name = locIdResolver
                CharacterNameBackgroundColor = ZColor.green.Brighten(.5f),
                DefaultPoseDevName = "DEFAULT",
                Poses = new List<CharacterPose>
                {
                    new CharacterPose
                    {
                        ExpressionDevName = "DEFAULT",
                        SpritePath = new []{"Characters", "ZJ"}
                    }
                }
            });
            
            characterConfigs.Add(new CharacterConfig
            {
                Id = id++,
                DevName = "PROTAG-M",
                //Name = locIdResolver.Register("You"),
                Size = CharacterSize.MEDIUM,
                // //Name = locIdResolver
                CharacterNameBackgroundColor = ZColor.green.Brighten(.5f),
                DefaultPoseDevName = "DEFAULT",
                Poses = new List<CharacterPose>
                {
                    new CharacterPose
                    {
                        ExpressionDevName = "DEFAULT",
                        SpritePath = new []{"Characters","Protag","Real","male_5-no_bg"}
                    }
                }
            });
            
            characterConfigs.Add(new CharacterConfig
            {
                Id = id++,
                DevName = "ZACHARI",
                //Name = locIdResolver.Register("Zachari"),
                Size = CharacterSize.MEDIUM,
                // //Name = locIdResolver
                CharacterNameBackgroundColor = ZColor.green.Brighten(.5f),
                DefaultPoseDevName = "DEFAULT",
                Poses = new List<CharacterPose>
                {
                    new CharacterPose
                    {
                        ExpressionDevName = "DEFAULT",
                        SpritePath = new []{"Characters","Protag","Real","male_5-no_bg"}
                    }
                }
            });
            
            characterConfigs.Add(new CharacterConfig
            {
                Id = id++,
                DevName = "PROTAG-F",
                //Name = locIdResolver.Register("You"),
                Size = CharacterSize.MEDIUM,
                // //Name = locIdResolver
                CharacterNameBackgroundColor = ZColor.green.Brighten(.5f),
                DefaultPoseDevName = "DEFAULT",
                Poses = new List<CharacterPose>
                {
                    new CharacterPose
                    {
                        ExpressionDevName = "DEFAULT",
                        SpritePath = new []{"Characters","Protag","Real","female_5-no_bg"}
                    }
                }
            });
            
            characterConfigs.Add(new CharacterConfig
            {
                Id = id++,
                DevName = "PROTAG-A",
                //Name = locIdResolver.Register("You"),
                Size = CharacterSize.MEDIUM,
                // //Name = locIdResolver
                CharacterNameBackgroundColor = ZColor.green.Brighten(.5f),
                DefaultPoseDevName = "DEFAULT",
                Poses = new List<CharacterPose>
                {
                    new CharacterPose
                    {
                        ExpressionDevName = "DEFAULT",
                        SpritePath = new []{"Characters","Protag","Real","Androgynous_5-no_bg"}
                    }
                }
            });

            #endregion

            #region NPCs

            characterConfigs.Add(new CharacterConfig
            {
                Id = id++,
                DevName = "BIGTEX",
                //Name = locIdResolver.Register("Big Tex"),
                Role = locIdResolver.Register("ORTHOPEDICS"),
                HasEpisodeTrack = true,
                Size = CharacterSize.LARGE,
                // //Name = locIdResolver
                CharacterNameBackgroundColor = ZColor.yellow.Brighten(.5f),
                DefaultPoseDevName = "DEFAULT",
                Poses = new List<CharacterPose>
                {
                    new CharacterPose
                    {
                        ExpressionDevName = "DEFAULT",
                        SpritePath = new []{"Characters", "Big_Tex"}
                    }
                }
            });

            characterConfigs.Add(new CharacterConfig
            {
                Id = id++,
                DevName = "ANNABEL",
                //Name = locIdResolver.Register("Annabel"),
                Role = locIdResolver.Register("VETERINARY"),
                HasEpisodeTrack = true,
                Size = CharacterSize.MEDIUM,
                
                CharacterNameBackgroundColor = ZColor.magenta.Brighten(.5f),
                DefaultPoseDevName = "DEFAULT",
                Poses = new List<CharacterPose>
                {
                    new CharacterPose
                    {
                        ExpressionDevName = "DEFAULT",
                        SpritePath = new []{"Characters", "Annabel"}
                    },
                }
            });
            characterConfigs.Add(new CharacterConfig
            {
                Id = id++,
                DevName = "HOLIDAY",
                //Name = locIdResolver.Register("Dr. Holiday"),
                Size = CharacterSize.MEDIUM,
                
                CharacterNameBackgroundColor = ZColor.blue.Brighten(.5f),
                
                DefaultPoseDevName = "DEFAULT",
                Poses = new List<CharacterPose>
                {
                    new CharacterPose
                    {
                        ExpressionDevName = "DEFAULT",
                        SpritePath = new []{"Characters", "Holiday"}
                    },
                }
            });
            characterConfigs.Add(new CharacterConfig
            {
                Id = id++,
                DevName = "DANTE",
                //Name = locIdResolver.Register("Dante"),
                Role = locIdResolver.Register("EMERGENCY"),
                HasEpisodeTrack = true,
                Size = CharacterSize.MEDIUM,
                
                CharacterNameBackgroundColor = ZColor.red.Brighten(.5f),
                
                DefaultPoseDevName = "DEFAULT",
                Poses = new List<CharacterPose>
                {
                    new CharacterPose
                    {
                        ExpressionDevName = "DEFAULT",
                        SpritePath = new []{"Characters", "Dante"}
                    },
                }
            });

            characterConfigs.Add(new CharacterConfig
            {
                Id = id++,
                DevName = "ZJ",
                //Name = locIdResolver.Register("ZJ"),
                Role = locIdResolver.Register("NEUROLOGY"),
                HasEpisodeTrack = true,
                CharacterNameBackgroundColor = ZColor.green.Brighten(.5f),
                
                DefaultPoseDevName = "DEFAULT",
                Poses = new List<CharacterPose>
                {
                    new CharacterPose
                    {
                        ExpressionDevName = "DEFAULT",
                        SpritePath = new []{"Characters", "ZJ"}
                    },
                }
            });

            #endregion

            #region Episode 1 Character

            
            characterConfigs.Add(new CharacterConfig
            {
                Id = id++,
                DevName = "WILLIAM",
                //Name = locIdResolver.Register("Billy"),
                Size = CharacterSize.SMALL,
                
                CharacterNameBackgroundColor = ZColor.red.Brighten(.5f),
                
                DefaultPoseDevName = "DEFAULT",
                Poses = new List<CharacterPose>
                {
                    new CharacterPose
                    {
                        ExpressionDevName = "DEFAULT",
                        SpritePath = new []{"Characters", "Kid"}
                    },
                }
            });
            
            characterConfigs.Add(new CharacterConfig
            {
                Id = id++,
                DevName = "STEPHANIE",
                //Name = locIdResolver.Register("Stephanie"),
                Size = CharacterSize.MEDIUM,
                
                CharacterNameBackgroundColor = ZColor.red.Brighten(.5f),
                
                DefaultPoseDevName = "DEFAULT",
                Poses = new List<CharacterPose>
                {
                    new CharacterPose
                    {
                        ExpressionDevName = "DEFAULT",
                        SpritePath = new []{"Characters", "Mother"}
                    },
                }
            });

            #endregion
            

            #endregion
            

            return characterConfigs;
        }

        public const string CONFIG_FILE_NAME = "config";
        public static List<CharacterConfig> FromResources()
        {
            var resources = ZSource.LoadAll<TextAsset>("Characters");

            // string test = Application.persistentDataPath;

            List<CharacterConfig> result = new List<CharacterConfig>();
            foreach (TextAsset textAsset in resources)
            {
                if (textAsset.name != CONFIG_FILE_NAME)
                {
                    ZBug.Warn("CHARACTER-CONFIG", $"File name not {CONFIG_FILE_NAME}: {textAsset.name}");
                    continue;
                }

                CharacterConfig characterConfig = ZConvert.FromJson<CharacterConfig>(textAsset.text);
                
                result.Add(characterConfig);
            }

            return result;
        }
    }
}