using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace ZEngine.Unity.Core.Messaging
{
    public static class MessageManagerExtension
    {
        public static void SendMessage(this Behaviour behaviour, IMessage message)
        {
            Send.Msg(message);
        }
    }

    public static class Send
    {
        public static void Msg<T>(T msg) where T : IMessage
        {
            MsgMgr.Instance.Send(msg);
        }

        public static void MsgNow<T>(T msg) where T : IMessage
        {
            MsgMgr.Instance.Send(msg, true);
        }
    }

    public class MsgMgr
    {
        private static MsgMgr gInstance;

        public static MsgMgr Instance
        {
            get
            {
                if (gInstance == null)
                {
                    gInstance = new MsgMgr();
                }

                return gInstance;
            }
            set => gInstance = value;
        }

        public delegate void MsgHandler<in T>(T message) where T : IMessage;

        private readonly ConcurrentDictionary<Type, List<object>> mMsgHandlers = new ConcurrentDictionary<Type, List<object>>();

        public List<IMessage> MessageBuffer = new List<IMessage>();
        
        public List<IMessage> stagingMsg = new List<IMessage>();
        public Queue<IMessage> inboundMsg = new Queue<IMessage>();

        public MsgMgr()
        {
            Init();
        }

        private MethodInfo mSendImmediateMethodInfo;
        private void Init()
        {
            mSendImmediateMethodInfo = typeof(MsgMgr).GetMethod("SendImmediate");
        }

        public void Pump()
        {
            foreach (IMessage stageMsg in stagingMsg)
            {
                inboundMsg.Enqueue(stageMsg);
            }
            stagingMsg.Clear();

            while (inboundMsg.Count > 0)
            {
                IMessage msg = inboundMsg.Dequeue();
                mSendImmediateMethodInfo.MakeGenericMethod(msg.GetType()).Invoke(this, new object[]
                {
                    msg
                });
                
                // SendImmediate(msg);
            };
        }

        public void Send<T>(T message, bool immediate = false) where T : IMessage
        {
            if (!immediate)
            {
                stagingMsg.Add(message);
            }
            else
            {
                SendImmediate(message);
            }
        }

        public void SendImmediate<T>(T message) where T : IMessage
        {
            MessageBuffer.Add(message);
            
            ZBug.Info("MSGMGR", $"[SEND]: {message.GetType().Name}");

            #if LOG_OBJECT
            try
            {
                ZBug.Info("MSGMGR", $"[SEND]: payload: {JsonConvert.SerializeObject(message)}");
            }
            catch (Exception)
            {
                // ignored
            }
            #endif

            // int msgType = message.MsgType;

            Type msgType = message.GetType();

            if (!mMsgHandlers.TryGetValue(msgType, out var handlers))
            {
                return;
            }

            lock (handlers)
            {
                try
                {
                    foreach (object o in handlers)
                    {
                        ((MsgHandler<T>) o).Invoke(message);
                    }
                }
                catch (InvalidOperationException e)
                {
                    ZBug.Warn("MSGMGR", $"InvalidOperation, something might be de-registering handlers as a result of a handler: {string.Join(",\n", MessageBuffer)}, e: {e}");
                    // throw;
                }
            }
        }
        
        public void SubscribeTo<T>(MsgHandler<T> msgHandler) where T : IMessage
        {
            Type msgType = typeof(T);

            // Debug.Log($"[MSGMGR] New subscribe to {msgType}, handler: {msgHandler}");
            var handlers = mMsgHandlers.GetOrAdd(msgType, OnAddNewHandlers<T>);

            lock (handlers)
            {
                handlers.Add(msgHandler);
            }
        }

        private static List<object> OnAddNewHandlers<T>(Type arg)
        {
            return new List<object>();
        }

        public void UnsubscribeFrom<T>(MsgHandler<T> msgHandler) where T : IMessage
        {
            Type msgType = typeof(T);
            ZBug.Info("MSGMGR", $"Unsubscribing from {msgType}, hanlder: {msgHandler}");

            if (!mMsgHandlers.TryGetValue(msgType, out var handlers))
            {
                return;
            }
            
            lock (handlers)
            {
                handlers.Remove(msgHandler);
            }
        }
    }
}