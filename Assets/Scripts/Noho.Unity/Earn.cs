using Noho.Configs;
using Noho.Unity.Messages;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity
{
    public static class Earn
    {
        public static void Score(string tag)
        {
            Send.Msg(new EarnScoreMsg
            {
                ScoreDelta = NohoConfigResolver.GetConfig<GunkConfig>(tag).PartialScore
            });
        }
    }
}