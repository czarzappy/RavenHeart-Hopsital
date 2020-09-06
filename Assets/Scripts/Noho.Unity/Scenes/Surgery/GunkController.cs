using System.Collections.Generic;
using System.Linq;
using Noho.Configs;
using Noho.Messages;
using Noho.Models;
using Noho.Parsing.Models;
using Noho.Unity.Components;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using Noho.Unity.Scenes.Surgery.Gunk;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Noho.Unity.Scenes.Surgery
{
    public class GunkController : MonoBehaviour
    {
        public GraphicRaycaster GraphicRaycaster;

        // public LinearGOPool NiceGOPool;

        public enum TestDataSet
        {
            NONE,
            A,
            B,
            C
        }
        
        [FormerlySerializedAs("Gunk")] 
        public List<MonoBehaviour> Gunks = new List<MonoBehaviour>();

        public SpawnerHemorrhageController SpawnerHemorrhage;
        public DefibrillatorController Defibrillator;
        public List<MonoBehaviour> SavedGunks = new List<MonoBehaviour>();
        
        public GunkInitData[] TestInitDataA;
        public GunkInitData[] TestInitDataB;
        public GunkInitData[] TestInitDataC;

        public bool FirstGunkTip = true;
        
        private readonly Dictionary<string, int> mProgress = new Dictionary<string, int>();
        
        private readonly Dictionary<string, List<MonoBehaviour>> mGunkTagLookup = new Dictionary<string, List<MonoBehaviour>>();

        public bool HasActiveGunk => mProgress.Count != 0;

        public void OnDestroy()
        {
            UnInit();
        }

        public void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<GunkClearedMsg>(OnClearedGunk);
            MsgMgr.Instance.UnsubscribeFrom<InitGunkMsg>(OnInitGunk);
            MsgMgr.Instance.UnsubscribeFrom<InitGunkIndicesMsg>(OnInitGunkIndices);
            MsgMgr.Instance.SubscribeTo<GunkAppearedMsg>(OnGunkAppeared);
        }
        
        public void Init(TestDataSet useTestData = TestDataSet.NONE)
        {
            // var settings = new PoolSettings
            // {
            //     PrefabResourcePath = 
            // };
            // NiceGOPool.Init(settings);
            
            MsgMgr.Instance.SubscribeTo<GunkClearedMsg>(OnClearedGunk);
            MsgMgr.Instance.SubscribeTo<InitGunkMsg>(OnInitGunk);
            MsgMgr.Instance.SubscribeTo<InitGunkIndicesMsg>(OnInitGunkIndices);
            MsgMgr.Instance.SubscribeTo<GunkAppearedMsg>(OnGunkAppeared);
            MsgMgr.Instance.SubscribeTo<ClearedSmallCutMsg>(OnClearedSmallCut);
            
            Gunks.Add(Defibrillator);
            Gunks.Add(SpawnerHemorrhage);
            Gunks.AddRange(SavedGunks);
            
            foreach (MonoBehaviour gunk in Gunks)
            {
                if (gunk == null)
                {
                    continue;
                }
                
                gunk.Hide();

                var gunkKey = gunk.tag;

                if (!mGunkTagLookup.TryGetValue(gunkKey, out List<MonoBehaviour> gunks))
                {
                    gunks = new List<MonoBehaviour>();
                    mGunkTagLookup[gunkKey] = gunks;
                }
                
                gunks.Add(gunk);
            }

            if (useTestData != TestDataSet.NONE)
            {
                GunkInitData[] initData = null;
                switch (useTestData)
                {
                    case TestDataSet.A:
                        initData = TestInitDataA;
                        break;
                    case TestDataSet.B:
                        initData = TestInitDataB;
                        break;
                    case TestDataSet.C:
                        initData = TestInitDataC;
                        break;
                }

                if (initData == null)
                {
                    ZBug.Warn("GUNK", $"No test data defined for test: {useTestData}");
                }
                else
                {
                    Send.Msg(new InitGunkMsg
                    {
                        InitData = initData,
                    });
                }
            }
        }

        private void OnClearedSmallCut(ClearedSmallCutMsg message)
        {
            foreach (MonoBehaviour gunk in Gunks)
            {
                if (gunk == message.SmallCut)
                {
                    Send.Msg(new GunkClearedMsg
                    {
                        Gunk = gunk
                    });
                    break;
                }
            }
        }

        /// <summary>
        /// Spawner related
        /// </summary>
        /// <param name="msg"></param>
        private void OnGunkAppeared(GunkAppearedMsg msg)
        {
            var gunk = msg.Gunk;
            var gunkKey = gunk.tag;

            if (!mGunkTagLookup.TryGetValue(gunkKey, out var gunks))
            {
                ZBug.Warn("GUNK", $"Attempting to reappear unknown gunk tag: {gunkKey}");
                return;
            }

            if (!gunks.Contains(gunk))
            {
                ZBug.Warn("GUNK", $"Attempting to registered gunk: {gunk}");
            }

            if (!mProgress.ContainsKey(gunkKey))
            {
                mProgress[gunkKey] = 0;
            }

            mProgress[gunkKey]++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        private void OnClearedGunk(GunkClearedMsg msg)
        {
            var gunk = msg.Gunk;
            var gunkKey = gunk.tag;

            if (!mGunkTagLookup.TryGetValue(gunkKey, out var gunks))
            {
                ZBug.Warn("GUNK", $"Attempting to clear unknown gunk tag: {gunkKey}");
                return;
            }

            if (!gunks.Contains(gunk))
            {
                ZBug.Warn("GUNK", $"Attempting to unregistered gunk: {gunk}");
            }
            
            Show.Nice(gunk.transform, transform);

            if (!msg.KeepAfterClear)
            {
                gunk.Hide();
            }

            if (!mProgress.ContainsKey(gunkKey))
            {
                ZBug.Info("GUNK", $"gunk is not known: {gunkKey}");
                return;
            }

            mProgress[gunkKey]--;
            
            Send.Msg(new EarnScoreMsg
            {
                ScoreDelta = NohoConfigResolver.GetConfig<GunkConfig>(gunkKey).CompletionScore,
            });

            if (mProgress[gunkKey] <= 0)
            {
                ZBug.Info("GUNK", $"Cleared all gunk of type: {gunkKey}");
                mProgress.Remove(gunkKey);

                if (mProgress.Count > 0)
                {
                    string nextGunkTag = mProgress.Keys.First();
                    ZBug.Info("GUNK", $"Next gunk tag: {nextGunkTag}");
                    Send.Msg(new GunkTotallyClearedMsg
                    {
                        GunkTag = gunkKey,
                        NextGunkTag = nextGunkTag
                    });
                }
                
            }
            else
            {
                ZBug.Info("GUNK", $"{mProgress[gunkKey]} remaining of gunk of type: {gunkKey}");
            }

            if (!HasActiveGunk)
            {
                ZBug.Info("GUNK", "CLEARED ALL GUNK!");
                Send.Msg(new ClearedAllGunkMsg());
            }
        }
        
        /// <summary>
        /// Activating an indexed gunk type
        /// </summary>
        /// <param name="message"></param>
        private void OnInitGunkIndices(InitGunkIndicesMsg message)
        {
            foreach (GunkInitIndicesData gunkInitIndicesData in message.InitData)
            {
                string key = gunkInitIndicesData.GunkTag;
                
                if (!mGunkTagLookup.TryGetValue(key, out var gunks))
                {
                    ZBug.Error("GUNK", $"[OnInitGunkIndices] Attempting to init unknown gunk tag: {key}");
                    continue;
                }

                int requiredProgress = 0;
                foreach (int index in gunkInitIndicesData.Indices)
                {
                    int trueIdx = index - 1;
                    if (gunks.Count <= trueIdx)
                    {
                        ZBug.Error("GUNK", $"[OnInitGunkIndices] Unknown gunk tag index: {key}, trueIdx: {trueIdx}, index: {index}");
                        continue;
                    }
                    if (gunks[trueIdx].IsHidden())
                    {
                        gunks[trueIdx].Show();
                        requiredProgress++;
                    }
                }

                if (FirstGunkTip)
                {
                    StartNewProcedureStep(key);
                }
                
                mProgress[key] = requiredProgress;
            }
        }

        private void StartNewProcedureStep(string gunkTag)
        {
            if (gunkTag == NohoConstants.GOTags.SPAWNER_HEMORRHAGE)
            {
                return;
            }

            Send.Msg(new StartNewProcedureStepMsg
            {
                Tag = gunkTag,
                Step = ProcedureStep.STEP_1
            });
            FirstGunkTip = false;
        }

        /// <summary>
        /// Activating an number of gunks
        /// </summary>
        /// <param name="message"></param>
        private void OnInitGunk(InitGunkMsg message)
        {
            foreach (GunkInitData gunkInitData in message.InitData)
            {
                string key = gunkInitData.GunkTag;
                
                if (!mGunkTagLookup.TryGetValue(key, out var gunks))
                {
                    ZBug.Error("GUNK", $"[OnInitGunk] Attempting to init unknown gunk tag: {key}");
                    continue;
                }
                ZBug.Info("GUNK", $"[OnInitGunk] gunk: {key} - {gunkInitData.Amount}");
                
                int amount = gunkInitData.Amount;

                for (int gunkIdx = 0; gunkIdx < gunks.Count && amount > 0; gunkIdx++)
                {
                    var gunk = gunks[gunkIdx];

                    if (gunk != null)
                    {
                        if (gunk.IsHidden())
                        {
                            gunk.Show();
                            amount--;
                        }
                    }
                }

                if (amount > 0)
                {
                    ZBug.Error("GUNK", $"Attempted to init too many of gunk tag: {key}, {amount} remaining out of {gunkInitData.Amount}, found count: {gunks.Count}");
                }
                
                if (FirstGunkTip)
                {
                    StartNewProcedureStep(key);
                }

                mProgress[key] = gunkInitData.Amount;
            }
        }

        public void DisableRaycasts()
        {
            GraphicRaycaster.Disable();
        }

        public void EnableRaycasts()
        {
            GraphicRaycaster.Enable();
        }

        public void InitEntity(Entity entity)
        {
            var prefab = ZSource.Load<RectTransform>(NohoResourcePack.FOLDER_SURGERIES, "Gunks", entity.Tag);

            UnityEngine.Vector2 anchoredPos = new UnityEngine.Vector2(entity.Position.X, entity.Position.Y);

            var rectTransform = Instantiate(prefab, Vector3.zero, Quaternion.Euler(0f, 0f, entity.Rotation), this.transform);

            rectTransform.localPosition = rectTransform.localPosition.ZeroZ();
            rectTransform.anchoredPosition = anchoredPos;
            
            SavedGunks.Add(rectTransform.GetComponent<MonoBehaviour>());

            if (rectTransform.CompareTag(NohoConstants.GOTags.HEMORRHAGE))
            {
                HemorrhageController hemorrhage = rectTransform.GetComponent<HemorrhageController>();
                SpawnerHemorrhage.AddSpawnSite(hemorrhage);
            }
        }

        public void Tick()
        {
            if (Defibrillator != null)
            {
                Defibrillator.Tick();
            }
        }
    }
}