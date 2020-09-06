using System.Collections.Generic;
using System.Linq;
using Noho.Messages;
using Noho.Unity.Components;
using Noho.Unity.Messages;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    public class BrokenRibController : MonoBehaviour
    {
        public List<BrokenFragmentController> BrokenFragments;

        public List<Transform> ScatterLocations;


        public Image FixedRibs;
        public List<Image> FixedImages = new List<Image>();
        public Image BrokenRibs;
        public void Start()
        {
            Init();
        }

        public void Init()
        {
            FixedImages.ForEach(image => image.Hide());
            
            BrokenRibs.Show();
            for (int i = 0; i < BrokenFragments.Count; i++)
            {
                var fragment = BrokenFragments[i];
                var scatter = ScatterLocations[i];
                
                fragment.Init(this.tag);
                
                var transform1 = fragment.transform;
                
                // place fragments in position
                transform1.position = scatter.position;
                transform1.rotation = scatter.rotation;
                
                
                fragment.Init2();
            }
            
            MsgMgr.Instance.SubscribeTo<FragmentPlacedMsg>(OnFragmentPlaced);
        }

        public void OnDestroy()
        {
            UnInit();
        }

        private void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<FragmentPlacedMsg>(OnFragmentPlaced);
        }

        private void OnFragmentPlaced(FragmentPlacedMsg message)
        {
            Show.Nice(message.Fragment.transform, transform);
            
            foreach (BrokenFragmentController fragment in BrokenFragments)
            {
                if (fragment.Phase != BrokenFragmentController.FragmentPhase.PLACED)
                {
                    return;
                }
            }
            
            // we are done!

            foreach (BrokenFragmentController brokenFragment in BrokenFragments)
            {
                brokenFragment.Hide();
            }
            
            FixedRibs.Show();
            FixedImages.ForEach(image => image.Show());
            BrokenRibs.Hide();

            Send.Msg(new GunkClearedMsg
            {
                Gunk = this,
                KeepAfterClear = true
            });
        }
    }
}