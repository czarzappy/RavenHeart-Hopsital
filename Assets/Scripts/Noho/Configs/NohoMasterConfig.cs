using System;
using System.Collections.Generic;
using ZEngine.Core.Configs;

namespace Noho.Configs
{
    public class NohoMasterConfig
    {
        public List<CharacterConfig> Characters;
        public List<ToolTipConfig> ToolTips;
        public List<GunkConfig> Gunks;

        public Dictionary<Type, List<MasterConfigItem>> Items;

        public void InitItems()
        {
            Items = new Dictionary<Type, List<MasterConfigItem>>();
            
            Items[typeof(CharacterConfig)] = new List<MasterConfigItem>(Characters);
            Items[typeof(ToolTipConfig)] = new List<MasterConfigItem>(ToolTips);
            Items[typeof(GunkConfig)] = new List<MasterConfigItem>(Gunks);
        }
        public List<MasterConfigItem> Get<T>()
        {
            return Items[typeof(T)];
        }
    }
}