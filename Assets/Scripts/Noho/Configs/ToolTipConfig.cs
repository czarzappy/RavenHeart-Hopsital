using System.Collections.Generic;
using Noho.Parsing.Models;
using ZEngine.Core.Configs;

namespace Noho.Configs
{
    public class ToolTipConfig : MasterConfigItem
    {
        public string Tag => this.DevName;
        
        public List<ToolTipSpeakerDef> Speakers = new List<ToolTipSpeakerDef>();
        
        public ToolTipSpeakerDef GetSpeaker(string characterDevName)
        {
            ToolTipSpeakerDef defaultDef = null;
            
            foreach (ToolTipSpeakerDef speakerDef in Speakers)
            {
                if (speakerDef.SpeakerDevName == characterDevName)
                {
                    return speakerDef;
                }

                if (speakerDef.SpeakerDevName == ToolTipDef.DEFAULT_SPEAKER_DEV_NAME)
                {
                    defaultDef = speakerDef;
                }
            }
            
            ZBug.Warn("TOOLTIP", $"Attempted to get speaker def for tag, {Tag}, unknown speaker: {characterDevName}");

            return defaultDef;
        }
    }
}