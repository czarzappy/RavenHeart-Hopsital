using Noho.Models;
using UnityEngine;
using ZEngine.Unity.Core.Attributes;

namespace Noho.Unity.Scenes.Surgery.Tools
{
    public class ScalpelController : MonoBehaviour
    {
        public Collider2D Collider2D;
        public TrailRenderer TrailRenderer;
    
        public void Awake()
        {
            Init();
        }

        private void Init()
        {
            // start off inactive
            DisableCutting();
            
            gameObject.AddZAttr<UIPointerFollowZAttr>();
        }

        public void Update()
        {
            if (!ToolboxController.IsEquippedCB(NohoConstants.ToolType.SCALPEL)())
            {
                DisableCutting();
                return;
            }
            
            // scalpel specific update logic
            if (Input.GetMouseButton(0))
            {
                EnableCutting();
            }
            else
            {
                DisableCutting();
            }
        }

        public void EnableCutting()
        {
            Collider2D.enabled = true;

            TrailRenderer.enabled = true;
            TrailRenderer.emitting = true;
        }

        public void DisableCutting()
        {
            Collider2D.enabled = false;

            TrailRenderer.emitting = false;
            TrailRenderer.enabled = false;
            TrailRenderer.Clear();
        }
    }
}
