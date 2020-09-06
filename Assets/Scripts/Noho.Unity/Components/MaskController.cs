using UnityEngine.UI;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Extensions;

namespace Noho.Unity.Components
{
    public class MaskController : UIMonoBehaviour
    {
        public Image Image;
        public Mask Mask;

        public void TurnOnMask()
        {
            Mask.Enable();
            Image.Enable();
        }
        
        public void TurnOffMask()
        {
            Mask.Disable();
            Image.Disable();
        }
    }
}