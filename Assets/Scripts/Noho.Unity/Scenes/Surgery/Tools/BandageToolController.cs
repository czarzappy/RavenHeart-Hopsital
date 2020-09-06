using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using ZEngine.Unity.Core.Attributes;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Extensions;

namespace Noho.Unity.Scenes.Surgery.Tools
{
    public class BandageToolController : MonoBehaviour, IPointerClickHandler, IPointerUpHandler
    {
        [FormerlySerializedAs("startPos")] 
        public Vector3 StartPos;

        public Vector3 CurrentPos;

        public Canvas Canvas;
        
        public bool IsBandaging = false;

        // public BoxCollider2D BoxCollider2D;

        public float BandageThickness = 2f;

        public Transform RollerDisplay;

        public float MinBandageDist = .5f;

        public LineComponent GauzeLine;

        public Vector3 localScale;
        public Vector3 globalScale;
        public void Start()
        {
            gameObject.AddZAttr<UIPointerFollowZAttr>();
            
            
            localScale = transform.root.localScale;
            
            
            globalScale = new Vector3(1 / localScale.x, 1 / localScale.y, 1f);

            GauzeLine.transform.localScale = globalScale;
        }

        public void OnEnable()
        {
            IsBandaging = false;
            //GauzeLine.Show();
        }

        public void OnDisable()
        {
            GauzeLine.DisablePhysics();
            GauzeLine.Hide();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnStartBandaging();
        }

        public void OnStartBandaging()
        {
            GauzeLine.DisablePhysics();
            StartPos = this.transform.position;
            StartPos.z = 0;
            IsBandaging = true;
            ZBug.Info("BANDAGE", "Start");
        }

        public Vector3 upDirection = Vector3.forward;

        private int frameWait = 0;
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnStartBandaging();
            }

            if (Input.GetMouseButtonUp(0))
            {
                OnStopBandaging();
                frameWait = 10;
            }

            if (!Input.GetMouseButton(0) && frameWait == 0)
            {
                GauzeLine.Hide();
            }
            else
            {
                frameWait--;
            }

            Vector3 lookForward;
            CurrentPos = transform.position;
            CurrentPos.z = 0;
            
            if (IsBandaging && Vector3.Distance(StartPos, CurrentPos) > MinBandageDist)
            {
                GauzeLine.Show();
                lookForward = StartPos - CurrentPos;
                lookForward.z = 0;
                
                GauzeLine.Init(StartPos, CurrentPos, BandageThickness);

                Quaternion quaternion = Quaternion.LookRotation(Vector3.forward, Vector3.up);

                Quaternion turnFromUpToForward = Quaternion.FromToRotation(Vector3.up, lookForward);

                RollerDisplay.rotation = quaternion * turnFromUpToForward;
            }
            else
            {
                // lookForward = Vector3.left;
            }

            // Quaternion quaternion = Quaternion.LookRotation(lookForward,
            //     upDirection);

            // quaternion = Quaternion.RotateTowards(quaternion, Quaternion.Euler(Vector3.forward), 0f);
            
            // quaternion.SetLookRotation(Vector3.forward);
        }

        public void OnStopBandaging()
        {
            ZBug.Info("BANDAGE", "Stop");
            GauzeLine.EnablePhysics(); // TODO: Move away from turning on and off the physics
            IsBandaging = false;
            // GauzeLine.Hide();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnStopBandaging();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            OnStartBandaging();
        }
    }
}