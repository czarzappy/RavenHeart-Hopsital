using System.Collections.Generic;
using Noho.Configs;
using Noho.Messages;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using UnityEngine;
using ZEngine.Unity.Core.Attributes;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    public class SpawnerHemorrhageController : MonoBehaviour
    {
        public List<HemorrhageController> SpawnSites = new List<HemorrhageController>();
        
        public void Start()
        {
            Init();
        }

        private ZAttr mSpawnerActivation;
        public void Init()
        {
            IsCleared = false;
            MsgMgr.Instance.SubscribeTo<GunkClearedMsg>(OnGunkCleared);
            MsgMgr.Instance.SubscribeTo<ClearedAllGunkMsg>(OnClearedAllGunk);
            
            mSpawnerActivation = this.gameObject.InitZAttr<ActivationAttr, ActivationAttr.InitData>(
                new ActivationAttr.InitData
                {
                    ActivationAction = Spawn,
                    ActivationDelay = NohoUISettings.SPAWN_HEMORRHAGE_DELAY,
                });
        }

        private void OnClearedAllGunk(ClearedAllGunkMsg message)
        {
            this.gameObject.RemoveZAttr(mSpawnerActivation);
        }

        public void OnDestroy()
        {
            UnInit();
        }

        private void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<GunkClearedMsg>(OnGunkCleared);
            MsgMgr.Instance.UnsubscribeFrom<ClearedAllGunkMsg>(OnClearedAllGunk);
        }

        private void OnGunkCleared(GunkClearedMsg message)
        {
            bool isOneOfMyGunks = false;
            foreach (HemorrhageController hemorrhageController in SpawnSites)
            {
                if (message.Gunk == hemorrhageController)
                {
                    isOneOfMyGunks = true;
                    break;
                }
            }

            if (!isOneOfMyGunks)
            {
                return;
            }
            
            Send.Msg(new EarnScoreMsg
            {
                ScoreDelta = NohoConfigResolver.GetConfig<GunkConfig>(this.tag).PartialScore
            });

            foreach (HemorrhageController hemorrhageController in SpawnSites)
            {
                if (!hemorrhageController.IsCleared)
                {
                    return;
                }
            }

            IsCleared = true;
            // all sites are cleared
            Send.Msg(new GunkClearedMsg
            {
                Gunk = this,
                KeepAfterClear = true, // allows for spawning
            });
        }

        public bool IsCleared;

        private void Spawn()
        {
            foreach (HemorrhageController hemorrhageController in SpawnSites)
            {
                if (hemorrhageController.IsHidden())
                {
                    ZBug.Warn("Spawning hemorrhage!");
                    hemorrhageController.Reset();
                    hemorrhageController.Show();

                    if (IsCleared)
                    {
                        IsCleared = false;
                        Send.Msg(new GunkAppearedMsg
                        {
                            Gunk = this
                        });
                    }
                    break;
                }
            }
        }

        public void AddSpawnSite(HemorrhageController hemorrhage)
        {
            SpawnSites.Add(hemorrhage);
        }
    }
}