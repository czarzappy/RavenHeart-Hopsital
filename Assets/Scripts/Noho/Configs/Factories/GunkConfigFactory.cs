using System.Collections.Generic;

namespace Noho.Configs.Factories
{
    public static class GunkConfigFactory
    {
        public static List<GunkConfig> Manual()
        {
            return new List<GunkConfig>
            {
                new GunkConfig
                {
                    CompletionScore = 100,
                    PartialScore = 10,
                    DevName = "default"
                }
            };
        }
    }
}