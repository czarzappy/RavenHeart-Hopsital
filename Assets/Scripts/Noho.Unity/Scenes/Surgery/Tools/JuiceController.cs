using UnityEngine;
using UnityEngine.EventSystems;
using ZEngine.Unity.Core.Attributes;

namespace Noho.Unity.Scenes.Surgery.Tools
{
    public class JuiceController : MonoBehaviour, IPointerDownHandler, IPointerClickHandler
    { 
        public ParticleSystemRenderer JuiceSystemRenderer;
        public ParticleSystem JuiceSystem;

        public Collider2D Collider2D;

        // Start is called before the first frame update
        void Start()
        {
            gameObject.AddZAttr<UIPointerFollowZAttr>();

            // juiceParticles.enabled = false;
            JuiceSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            Collider2D.enabled = false;
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            SprayJuice();
        }

        public void Update()
        {
            if (Input.GetMouseButton(0))
            {
                SprayJuice();
            }
            else
            {
                JuiceSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                Collider2D.enabled = false;
            }
        }

        public void SprayJuice()
        {
            if (!JuiceSystem.isPlaying)
            {
                JuiceSystem.Play();
                Collider2D.enabled = true;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SprayJuice();
        }
    }
}