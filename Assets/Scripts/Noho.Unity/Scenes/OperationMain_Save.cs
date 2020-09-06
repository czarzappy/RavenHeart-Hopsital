using System.Collections.Generic;
using System.IO;
using Noho.Parsing.Models;
using Noho.Parsing.Parsers;
using Noho.Unity.Models;
using UnityEngine;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Models;
using Vector2 = System.Numerics.Vector2;

namespace Noho.Unity.Scenes
{
    public partial class OperationMain
    {
        private void Load(SceneDef sceneDef)
        {
            ZBug.Info("Operation", "Loading!");

            Anatomy.Show();

            ResourcePath path = new ResourcePath(NohoResourcePack.FOLDER_SURGERIES, "Anatomy", sceneDef.Name);
            
            ZBug.Info("Operation", $"Background asset path: {path}");
            
            var sprite = ZSource.Load<Sprite>(path);
            
            ZBug.Info("Operation", $"Background sprite: {sprite}");

            Anatomy.sprite = sprite;

            AnatomyLoad(sceneDef);
            
            OperationBackgroundController.Init(sceneDef.BgType, sceneDef.BgOffset, sceneDef.OverrideBgSize);
            
            foreach (var entity in sceneDef.Entities)
            {
                GunkController.InitEntity(entity);
            }
        }

        private void AnatomyLoad(SceneDef sceneDef)
        {
            if (sceneDef.BgOffset.X != 0 || sceneDef.BgOffset.Y != 0)
            {
                Anatomy.rectTransform.anchoredPosition = new UnityEngine.Vector2(sceneDef.BgOffset.X, sceneDef.BgOffset.Y);
            }

            if (sceneDef.OverrideBgSize.X != 0 || sceneDef.OverrideBgSize.Y != 0)
            {
                Anatomy.rectTransform.sizeDelta = new UnityEngine.Vector2(sceneDef.OverrideBgSize.X, sceneDef.OverrideBgSize.Y);
            }
            else
            {
                Anatomy.SetNativeSize();
            }
        }
        
        public void AnatomySave(SceneDef sceneDef)
        {
            var sizeDelta = Anatomy.rectTransform.sizeDelta;
            var anchoredPosition = Anatomy.rectTransform.anchoredPosition;
            
            if (anchoredPosition.x != 0 || anchoredPosition.y != 0)
            {
                sceneDef.BgOffset = new Vector2(anchoredPosition.x, anchoredPosition.y);
            }

            if (sizeDelta.x != 0 || sizeDelta.y != 0)
            {
                sceneDef.OverrideBgSize = new Vector2(sizeDelta.x, sizeDelta.y);
            }
        }
        
        public void Save()
        {
            ZBug.Info("Operation", "Saving!");
            SceneDef sceneDef = new SceneDef();
            
            List<Entity> entities = new List<Entity>();

            sceneDef.Name = SurgerySceneName;
            sceneDef.Entities = entities;
            
            sceneDef.BgType = OperationBackgroundController.BGType;

            AnatomySave(sceneDef);
            
            // sceneDef.i

            foreach (MonoBehaviour monoBehaviour in GunkController.SavedGunks)
            {
                if (monoBehaviour == null)
                {
                    continue;
                }
                
                var rectTransform = monoBehaviour.GetComponent<RectTransform>();
                
                UnityEngine.Vector2 anchoredPos = rectTransform.anchoredPosition;

                Vector3 rot = rectTransform.rotation.eulerAngles;
                
                Entity entity = new Entity
                {
                    Tag = monoBehaviour.tag,
                    Position = new Vector2(anchoredPos.x, anchoredPos.y),
                    Rotation = rot.z
                };
                
                entities.Add(entity);
            }

            string filePath = Path.Combine("Assets","Resources", NohoResourcePack.FOLDER_SURGERIES, "Scenes", $"{SurgerySceneName}.xml");

            using (var stream = File.Create(filePath))
            {
                using (TextWriter textWriter = new StreamWriter(stream))
                {
                    SceneConvert.ToXmlStream(textWriter, sceneDef);
                }
            }

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
    }
}