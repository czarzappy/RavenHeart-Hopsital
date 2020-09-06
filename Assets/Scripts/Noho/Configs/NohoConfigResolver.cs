using System.Collections.Generic;
using System.Linq;
using Noho.Configs.Factories;
using ZEngine.Core.Configs;
using ZEngine.Core.Localization;

namespace Noho.Configs
{
    public class NohoConfigResolver : ConfigIdResolver
    {
        public static NohoMasterConfig MasterConfig;

        static NohoConfigResolver()
        {
            Init();
        }

        public static void ReInit()
        {
            UnInit();
            Init();
        }

        private static void UnInit()
        {
        }

        public static void Init()
        {
            var locIdResolver = new DictionaryLocIdResolver();
            
            locIdResolver.SetLanguage(NohoLang.ENGLISH);

            LocIdResolver.Instance = locIdResolver;
            
            MasterConfig = new NohoMasterConfig
            {
                Characters = CharacterConfigFactory.FromResources(),
                ToolTips = ToolTipConfigFactory.FromResources(),
                Gunks = GunkConfigFactory.Manual(),
                // Characters = CharacterConfigFactory.GenerateCharacters(locIdResolver)
            };

            MasterConfig.InitItems();
        }
        
        public override TConfig Resolve<TConfig>(ConfigId<TConfig> configId)
        {
            List<MasterConfigItem> items = MasterConfig.Get<TConfig>();

            foreach (MasterConfigItem item in items)
            {
                if (item.Id == configId.Id)
                {
                    return (TConfig) item;
                }
            }

            return null;
        }

        public static IEnumerable<TConfig> GetConfigs<TConfig>() where TConfig : MasterConfigItem
        {
            List<MasterConfigItem> items = MasterConfig.Get<TConfig>();

            return items.Select((item) => (TConfig) item);
        }

        public static TConfig GetConfig<TConfig>(string configDevName) where TConfig : MasterConfigItem
        {
            List<MasterConfigItem> items = MasterConfig.Get<TConfig>();

            foreach (MasterConfigItem item in items)
            {
                if (item.CompareDevName(configDevName))
                {
                    return (TConfig) item;
                }
            }

            ZLog.Warn($"No {typeof(TConfig)} config found, name: {configDevName}");
            return null;
        }
    }
}