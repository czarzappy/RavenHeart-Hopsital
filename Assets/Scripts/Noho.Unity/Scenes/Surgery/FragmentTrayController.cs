using System.Collections.Generic;
using Noho.Unity.Messages;
using Noho.Unity.Scenes.Surgery.Gunk;
using UnityEngine;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery
{
    public class FragmentTrayController : UIMonoBehaviour
    {
        public List<FragmentSpotController> FragmentSpots;

        public Transform Display;

        public Transform Gunks;
        
        public void Start()
        {
            Init();
        }

        public void Init()
        {
            Display.Hide();
            MsgMgr.Instance.SubscribeTo<FragmentClearedMsg>(OnFragmentCleared);
            MsgMgr.Instance.SubscribeTo<FragmentPlacedMsg>(OnFragmentPlaced);
        }

        public void OnDestroy()
        {
            UnInit();
        }

        private void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<FragmentClearedMsg>(OnFragmentCleared);
            MsgMgr.Instance.UnsubscribeFrom<FragmentPlacedMsg>(OnFragmentPlaced);
        }

        private void OnFragmentPlaced(FragmentPlacedMsg message)
        {
            bool hasAny = false;
            foreach (FragmentSpotController fragmentSpot in FragmentSpots)
            {
                if (fragmentSpot.Occupant == message.Fragment)
                {
                    fragmentSpot.Vacate();
                }

                if (!fragmentSpot.IsOpen)
                {
                    hasAny = true;
                    break;
                }
            }

            if (!hasAny)
            {
                Display.Hide();
            }
        }


        private void OnFragmentCleared(FragmentClearedMsg message)
        {
            Display.Show();
            
            foreach (FragmentSpotController fragmentSpot in FragmentSpots)
            {
                if (!fragmentSpot.IsOpen)
                {
                    continue;
                }
                
                fragmentSpot.Occupy(Gunks, message.Fragment);
                break;
            }
        }
    }
}