using System;
using System.Linq;
using Noho.Models;
using Noho.Parsing.Models;

namespace Noho.Extensions
{
    public static class SceneDefExt
    {
        public static int NumEntitiesWithTag(this SceneDef sceneDef, string tag)
        {
            switch (tag)
            {
                case NohoConstants.GOTags.DEFIBRILLATOR:
                case NohoConstants.GOTags.SPAWNER_HEMORRHAGE:
                case NohoConstants.GOTags.SYRINGE:
                    return 1;
            }
            
            return sceneDef.Entities.Count(entity => entity.Tag == tag);
        }
    }
}