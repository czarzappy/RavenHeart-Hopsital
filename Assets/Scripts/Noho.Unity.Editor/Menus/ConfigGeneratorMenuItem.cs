using System.IO;
using Newtonsoft.Json;
using Noho.Configs;
using Noho.Configs.Factories;
using UnityEditor;
using ZEngine.Core.Localization;
using ZEngine.Unity.Core.IO;
using ZEngine.Unity.Core.Serialization;

namespace Noho.Unity.Editor.Menus
{
    public static class ConfigGeneratorMenuItem
    {
        public const string PATH = "Assets/Noho/Export Character Configs";
        
        [MenuItem(PATH)]
        public static void Execute()
        {
            var locIdResolver = new DictionaryLocIdResolver();
            
            locIdResolver.SetLanguage(NohoLang.ENGLISH);
            
            var configs = CharacterConfigFactory.GenerateCharacters(locIdResolver);

            foreach (CharacterConfig characterConfig in configs)
            {
                // var json = JsonConvert.SerializeObject(characterConfig, Formatting.Indented);
                var json = ZConvert.ToJson(characterConfig);
                
                ZBug.Info(json);

                string charPath = Path.Combine("Assets", "Resources", "Characters", characterConfig.DevName);
                ZDir.CreateDirIfNotExists(charPath);
                // Directory.CreateDirectoryIfNotExists();
                string configPath = Path.Combine(charPath, $"{CharacterConfigFactory.CONFIG_FILE_NAME}.json");
                File.WriteAllText(configPath, json);
            }
            
            AssetDatabase.Refresh();
        }

        [MenuItem(PATH, true)]
        public static bool Validate()
        {
            return true;
        }
    }
}