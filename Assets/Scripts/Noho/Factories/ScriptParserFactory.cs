using System.Collections.Generic;
using System.Linq;
using Noho.Configs;
using Noho.Models;
using Noho.Parsing.Models;
using Noho.Parsing.Parsers;
using ZEngine.Core.Dialogue.Parsers;

namespace Noho.Factories
{
    public static class ScriptParserFactory
    {
        public static IScriptParser DefaultParser()
        {
            var characterNames = NohoConfigResolver.MasterConfig.Characters.Select((config => config.DevName)).ToList();
            
            characterNames.Add(NohoParsingConstants.CHARACTER_DEV_NAME_NARRATOR);
            characterNames.Add(NohoParsingConstants.CHARACTER_DEV_NAME_PATIENT);
            characterNames.Add(NohoParsingConstants.CHARACTER_DEV_NAME_EDGAR);


            var parserSettings = new ParserSettings()
            {
                KnownSpeakerNames = characterNames
            };
            
            var phaseParserSettings = new PhaseParserSettings
            {
                KnownSpeakerNames = characterNames,
                KnownInitGunks = new List<string>
                {
                    "WOUND_GLASS",
                    "WOUND_STEEL_SMALL",
                    "WOUND_CUT_SMALL",
                    "WOUND_CUT_LARGE",
                    "HEMORRHAGE",
                    "ULCER_SMALL",
                    "ULCER_LARGE",
                    "BUILDO_BRICKS",
                },
                KnownIndexedGunks = new List<string>
                {
                    "SYRINGE",
                    "SCALPEL",
                    "STITCH_GEL_BANDAGE",
                    "SUTURE",
                    "SPAWNER_HEMORRHAGE",
                    "BROKEN_RIBS",
                    "BROKEN_BONE_CAST",
                    "DEFIBRILLATOR",
                    "BROKEN_CAT_LEG",
                    "BROKEN_CAT_LEG_CAST",
                    "BROKEN_BONE_CAST",
                    "BROKEN_ARM", // deprecated
                    "BROKEN_ARM_BONE_CAST",
                    "BROKEN_LEG_BONE_CAST",
                }
            };
            
            var parser = new HierarchicalScriptParser();
            
            parser.Register(new PhaseDefParser(phaseParserSettings));
            parser.Register(new OperationDefParser());
            parser.Register(new PatientDefParser());
            parser.Register(new CutSceneDefParser(parserSettings));
            parser.Register(new BriefingDefParser());
            parser.Register(new ActionChoiceDefParser());
            parser.Register(new ChoiceDefParser());

            parser.Register(new LoveMinDefParser());
            parser.Register(new PreOpDefParser());
            parser.Register(new NewPatientDefParser(parserSettings));
            parser.Register(new CheckFlagDefParser());

            return parser;
        }
    }
}