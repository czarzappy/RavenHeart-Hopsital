using System.IO;
using Noho.Parsing.Models;
using Noho.Parsing.Parsers;
using Noho.Unity.Models;
using UnityEngine;
using ZEngine.Unity.Core;

namespace Noho.Unity.Factories
{
    public static class SceneDefFactory
    {
        public static SceneDef FromResources(string surgerySceneName)
        {
            TextAsset sceneText = ZSource.Load<TextAsset>(NohoResourcePack.FOLDER_SURGERIES, "Scenes", surgerySceneName);
                    
            TextReader reader = new StringReader(sceneText.text);
                
            SceneDef sceneDef = SceneConvert.FromTextReader(reader);

            return sceneDef;
        }
        
    }
}