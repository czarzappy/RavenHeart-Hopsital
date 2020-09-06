using System.Collections.Generic;
using UnityEngine;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    public class FailBoundController : MonoBehaviour
    {
        public List<Collider2D> FailBounds;

        public void TurnOnFailBounds()
        {
            foreach (var failBound in FailBounds)
            {
                failBound.enabled = true;
            }
        }
        
        public void TurnOffFailBounds()
        {
            foreach (var failBound in FailBounds)
            {
                failBound.enabled = false;
            }
        }
    }
}