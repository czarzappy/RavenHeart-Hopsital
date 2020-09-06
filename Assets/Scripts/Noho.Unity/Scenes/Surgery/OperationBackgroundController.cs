using Noho.Parsing.Models;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core.Components;
using Vector2 = System.Numerics.Vector2;

namespace Noho.Unity.Scenes.Surgery
{
    public class OperationBackgroundController : UIMonoBehaviour
    {
        public Image Image;

        public Material InternalMat;
        public Material ExternalMat;
        public BackgroundType BGType;

        private const bool USE_SPECIAL_MATERIALS = true;
        public void Init(BackgroundType backgroundType, Vector2 sceneDefBgOffset, Vector2 sceneDefOverrideBgSize)
        {
            ZBug.Info("Scroll", $"background type: {backgroundType}");

            if (USE_SPECIAL_MATERIALS)
            {
                switch (backgroundType)
                {
                    case BackgroundType.Internal:
                        Image.material = InternalMat;
                        break;
                    case BackgroundType.External:
                        Image.material = ExternalMat;
                        break;
                }
            }

            BGType = backgroundType;
        }
    }
}
