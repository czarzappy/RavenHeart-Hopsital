using UnityEngine;
using ZEngine.Unity.Core.Attributes;

namespace Noho.Unity.Scenes.Surgery.Tools
{
    public class ForcepsToolController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            gameObject.AddZAttr<UIPointerFollowZAttr>();
        }
    }
}
