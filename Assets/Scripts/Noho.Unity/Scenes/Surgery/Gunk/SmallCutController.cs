using Noho.Models;
using Noho.Unity.Messages;
using UnityEngine;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    public class SmallCutController : MonoBehaviour
    {
        // Start is called before the first frame update

        public GameObject ObstructionGO;

        private bool mInit = false;
        void Start()
        {
            if (!mInit)
            {
                Init(null);
            }
        }

        public void Init(GameObject obstructionGO)
        {
            mInit = true;
            ObstructionGO = obstructionGO;
            FailBound.TurnOnFailBounds();
        }

        public FailBoundController FailBound;

        private int mObstructionCount = 0;

        public bool IsObstructed => mObstructionCount > 0;

        private bool mCleared = false;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (mCleared)
            {
                return;
            }
            
            switch (other.gameObject.tag)
            {
                case NohoConstants.GOTags.JUICE:
                    
                    if (!IsObstructed)
                    {
                        ZBug.Info("SMALLCUT",$"GOT JUICED!, count: {mObstructionCount}");
                        
                        mCleared = true;
                        Send.Msg(new ClearedSmallCutMsg
                        {
                            SmallCut = this
                        });
                        // Destroy(this.gameObject);
                    }
                    break;
                
                // case NohoConstants.GOTags.GLASS:
                case NohoConstants.GOTags.OBSTRUCTION:

                    if (!IsObstructingGO(other.gameObject))
                    {
                        return;
                    }

                    // means obstruction must not be moving
                    if (!Input.GetMouseButtonUp(0))
                    {
                        break;
                    }

                    AddObstruction();
                
                    // Vector2 closestPoint = other.ClosestPoint(transform.position);
                    
                    // Debug.DrawLine(transform.position, closestPoint, Color.green, 10000);
                    // Debug.DrawLine(closestPoint, other.transform.position, Color.green, 10000);
                    // Debug.DrawLine(transform.position, other.transform.position, Color.red, 10000);
                    ZBug.Info($"{this} small cut entered by: {other.gameObject} count: {mObstructionCount}");
                    break;
            }
        }

        public void AddObstruction()
        {
            mObstructionCount++;

            Send.Msg(new SmallCutObstructedMsg
            {
                SmallCut = this
            });
        }

        private bool IsObstructingGO(GameObject gameObject)
        {
            if (ObstructionGO == null)
            {
                return false;
            }

            return gameObject == ObstructionGO;
        }

        void OnParticleTrigger()
        {
            Debug.Log("small cut particle hit!");
        }
        
        void OnTriggerExit2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case NohoConstants.GOTags.OBSTRUCTION:

                    if (!IsObstructingGO(other.gameObject))
                    {
                        return;
                    }
                    
                    mObstructionCount--;
                    Debug.Log($"{this} small cut exited by: {other.gameObject} count: {mObstructionCount}");

                    if (!IsObstructed)
                    {
                        Send.Msg(new SmallCutUnobstructedMsg
                        {
                            SmallCut = this
                        });
                        
                        FailBound.TurnOffFailBounds();
                    }
                    break;
            }
        }
    }
}
