using Noho.Models;
using Noho.Unity.Messages;
using UnityEngine;
using ZEngine.Unity.Core.Attributes;
using ZEngine.Unity.Core.Collections.GOPools;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;
using ZEngine.Unity.Core.Models;

namespace Noho.Unity.Scenes.Surgery.Tools
{
    public class SutureController : MonoBehaviour
    {
        public LinearGOPool LinePool;

        public float MinSutureLength;
        public float SutureThickness;

        public float NewSutureEulerThreshold;

        private bool mDrawNextFrame;
        
        public Vector3 LastStartPos;
        private Vector3 mLastDirection;

        // private bool mIsFirstSuture = true;
        
        // public Vector3[];
        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        public void Init()
        {
            gameObject.AddZAttr<UIPointerFollowZAttr>();

            PoolSettings poolSettings = new PoolSettings
            {
                PrefabResourcePath = LineComponent.RESOURCE_PATH,
                Overflow = new MaxOverflowSettings
                {
                    HardCap = 20,
                    SoftCap = 10,
                },
                ParentTransform = this.transform.parent
            };

            LinePool.Init(poolSettings);
            
            MsgMgr.Instance.SubscribeTo<ClearSuturesMsg>(OnClearSuturesMsg);
        }

        public void OnDestroy()
        {
            UnInit();
        }

        public void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<ClearSuturesMsg>(OnClearSuturesMsg);
        }

        private void OnClearSuturesMsg(ClearSuturesMsg message)
        {
            RemoveSutures();
        }

        // Update is called once per frame

        private LineComponent headLine;
        void Update()
        {
            if (ToolboxController.Instance.CurrentToolType != NohoConstants.ToolType.SUTURE)
            {
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                RemoveSutures();
                LastStartPos = transform.position;
                LastStartPos.z = 0;
                
                headLine = LinePool.Get<LineComponent>();
                return;
            }
        
            if (Input.GetMouseButton(0))
            {
                DrawSutures();
            }

            if (Input.GetMouseButtonUp(0))
            {
                FinishSuture();
            }
        }

        public void FinishSuture()
        {
            // var position = transform.position;
            // position.z = 0;
            
            // LinePool.Get<LineComponent>().Init(LastStartPos, 
            //     position,
            //     SutureThickness, 
            //     Color.green, 
            //     Color.white);
            //
            // LinePool.Return(headLine);
            LinePool.ReturnAll();
            Send.Msg(new SutureFinishedMessage());
        }

        public float NewAngle;

        public Vector3 LastPos;

        public float Distance;
        void DrawSutures()
        {
            var position = transform.position;
            position.z = 0;

            Distance = Vector3.Distance(LastStartPos, position);
            if (!(Distance < MinSutureLength))
            {
                Vector3 direction = VectorExt.GetNormalDirection(LastPos, position);

                // bool makeSuture = true;
                // bool isTrailing = true;
                // if (Vector3.Distance(mLastPos, position) > )
                // if(true)
                // {
                //     
                //     
                // }
                // if (!mIsFirstSuture)
                // {
                NewAngle = Vector3.Angle(direction, mLastDirection);

                if (NewAngle > NewSutureEulerThreshold)
                {
                    var line = LinePool.Get<LineComponent>();
                    line.Init(LastStartPos, 
                        position, 
                        SutureThickness, 
                        Color.green,
                        Color.white);

                    line.gameObject.tag = NohoConstants.GOTags.SUTURE;

                    LastStartPos = position;
                }
                //
                //     if (newAngle < NewSutureEulerThreshold)
                //     {
                //         makeSuture = false;
                //     }
                //     else
                //     {
                //         Debug.Log($"angle: {newAngle}");
                //     }
                // }
                // else
                // {
                //     mIsFirstSuture = false;
                //     return;
                // }

                LastPos = position;
                // mLastDirection = direction;
            }
            else
            {
                mLastDirection = VectorExt.GetNormalDirection(LastStartPos, position);
            }

            headLine.Init(LastStartPos, 
                position, 
                SutureThickness,
                 Color.red,
                Color.red);
            headLine.Show();

            // if (makeSuture)
            // {
            //     mLastStartPos = position;
            //     
            //     mLastDirection = mLastStartPos - position;
            // }

            // if (mIsFirstSuture)
            // {
            //     mIsFirstSuture = false;
            // }
        }

        void RemoveSutures()
        {
            // mIsFirstSuture = true;
            
            LinePool.ReturnAll();
        }
    }
}
