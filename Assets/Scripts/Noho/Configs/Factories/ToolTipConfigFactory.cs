using System.Collections.Generic;
using System.Linq;
using Noho.Parsing.Models;
using Noho.Parsing.Parsers;
using UnityEngine;
using ZEngine.Core.Dialogue.Models;
using ZEngine.Core.Dialogue.Parsers;
using ZEngine.Core.Utils;
using ZEngine.Unity.Core;

namespace Noho.Configs.Factories
{
    public static class ToolTipConfigFactory
    {
        public static List<ToolTipConfig> FromResources()
        {
            IScriptParser scriptParser = new HierarchicalScriptParser();
            scriptParser.Register(new ToolTipDefParser());
            var resources = ZSource.LoadAll<TextAsset>("tooltips");

            Dictionary<string, ToolTipConfig> tipConfigs = new Dictionary<string, ToolTipConfig>();
            
            foreach (TextAsset tooltipFile in resources)
            {
                var lines = LineUtil.GetLines(tooltipFile.text).AsParserLines();

                var tips = scriptParser.Deserialize<ToolTipDef>(lines);

                foreach (ToolTipDef toolTipDef in tips)
                {
                    string key = toolTipDef.Tag;
                    
                    if (!tipConfigs.TryGetValue(key, out ToolTipConfig config))
                    {
                        config = new ToolTipConfig
                        {
                            DevName = key,
                        };
                        
                        tipConfigs.Add(key, config);
                    }
                    
                    config.Speakers.AddRange(toolTipDef.Speakers);
                }
            }

            return tipConfigs.Values.ToList();
        }
    }
}