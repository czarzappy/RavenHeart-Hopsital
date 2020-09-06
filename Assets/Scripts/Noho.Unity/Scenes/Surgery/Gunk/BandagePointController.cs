using Noho.Models;
using Noho.Unity.Messages;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI.UIShape;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    public class BandagePointController : MonoBehaviour
    {
        [FormerlySerializedAs("mCoverCount")] public int CoverCount = 0;
        public bool IsCovered => CoverCount > 0;

        public UIShape UIShape;
        public Color GoodColor;
        public Color BadColor;

        // private UnityAction<BandagePointController> mOnExit;

        public void Init()
        {
            ZBug.Info("BANDAGEPOINT", "Init");
            // mOnExit = onExit;
        }
        public void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case NohoConstants.GOTags.BANDAGE:
                    if (IsCovered)
                    {
                        return;
                    }
                    
                    ZBug.Info("BANDAGEPOINT", "ENTER");
                    CoverCount++;

                    UIShape.color = (IsCovered) ? GoodColor : BadColor;

                    Send.MsgNow(new BandageCoveredMsg
                    {
                        Point = this,
                        Bandage = other.gameObject
                    });
                    break;
            }
        }
        public void OnTriggerExit2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case NohoConstants.GOTags.BANDAGE:
                    ZBug.Info("BANDAGEPOINT", "EXIT");
                    CoverCount--;
                    
                    UIShape.color = (IsCovered) ? GoodColor : BadColor;

                    // mOnExit(this);
                    break;
            }
        }
    }
}