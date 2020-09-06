using System.Collections.Generic;
using Noho.Configs;
using Noho.Configs.Factories;
using Noho.Models;
using UnityEngine;
using ZEngine.Unity.Core;
using ZEngine.Unity.Steam;

namespace Noho.Unity.Scenes
{
    public class ImportMain : MonoBehaviour
    {
        public void Start()
        {
            ZBug.Info("Import", "Init Steam");
            ZSteamClient.Init(NohoConstants.STEAM_DEMO_APP_ID);
        }

        public List<CharacterConfig> CharacterConfigs = new List<CharacterConfig>();

        public void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                ZBug.Info("Import", "Mouse down");
                DoImport();
            }
        }

        public void OnDestroy()
        {
            ZBug.Info("Import", "Steam shutdown");
            ZSteamClient.Shutdown();
        }

        private void DoImport()
        {
            ZBug.Info("Import", "Do Import");
            // Resources.
            var resources = ZSource.LoadAll<TextAsset>("Characters");

            // string test = Application.persistentDataPath;

            CharacterConfigs = CharacterConfigFactory.FromResources();
        }
    }
}