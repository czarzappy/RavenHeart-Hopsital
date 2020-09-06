using Noho.Messages;
using Noho.Unity.Messages;
using UnityEngine;
using ZEngine.Unity.Core.Messaging;
using static Noho.Models.NohoConstants;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    public class LargeCutController : MonoBehaviour
    {
        public int NumberOfSuturesRequired;

        private int mSutureCount = 2;
        private bool mCleared;

        public void Awake()
        {
            Init();
        }
        
        public void Init()
        {
            MsgMgr.Instance.SubscribeTo<SutureFinishedMessage>(OnSutureFinishedMessage);
            
            NumberOfSuturesRequired = 2;
            mCleared = false;
            mSutureCount = 0;
        }

        public void OnDestroy()
        {
            UnInit();
        }

        private void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<SutureFinishedMessage>(OnSutureFinishedMessage);
        }

        private void OnSutureFinishedMessage(SutureFinishedMessage message)
        {
            if (mSutureCount > NumberOfSuturesRequired && !mCleared)
            {
                mCleared = true;
                Send.Msg(new ClearSuturesMsg());
                Send.Msg(new GunkClearedMsg
                {
                    Gunk = this
                });
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case GOTags.SUTURE:

                    mSutureCount++;
                    // Debug.Log($"large cut trigger enter suture: {sutureCount}");
                    
                    break;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            // Debug.Log("large cut trigger exit");
        }
    }
}
