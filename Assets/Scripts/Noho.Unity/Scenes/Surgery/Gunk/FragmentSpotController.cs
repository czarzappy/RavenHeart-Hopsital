using UnityEngine;
using ZEngine.Unity.Core.Components;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    public class FragmentSpotController : UIMonoBehaviour
    {
        public bool IsOpen => Occupant == null;

        public GameObject Occupant;
        
        public void Occupy(Transform parent, GameObject fragment)
        {
            // doing this re-parenting to deal with the UI issues
            ZBug.Info("Fragment Spot, occupying...");
            Occupant = fragment;
            var fragTrans = fragment.transform;
            var thisTrans = transform;
            
            fragTrans.position = thisTrans.position;
            fragTrans.SetParent(parent, true);
            
            // the tray is lerping this away, need to wait
            // brokenFragment.rectTransform.anchoredPosition = this.rectTransform.anchoredPosition;
        }

        public void Vacate()
        {
            Occupant = null;
        }
    }
}