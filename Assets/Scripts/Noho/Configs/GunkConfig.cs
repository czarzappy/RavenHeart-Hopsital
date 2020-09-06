using ZEngine.Core.Configs;

namespace Noho.Configs
{
    public class GunkConfig : MasterConfigItem
    {
        public int CompletionScore;
        public int PartialScore;

        public override bool CompareDevName(string devName)
        {
            return true;
        }
    }
}