using ZEngine.Unity.Core.Messaging;

namespace Noho.Messages
{
    public abstract class BaseNohoMessage : IMessage
    {
        public abstract MessageConstants.NohoMsgType NohoMsgType { get; }
        
        public int MsgType => (int) NohoMsgType;
    }
}