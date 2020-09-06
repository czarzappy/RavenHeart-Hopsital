using UnityEngine;

namespace ZEngine.Unity.Core
{
    public static class ZInput
    {
        public static Vector3 mouseWorldPosition
        {
            get
            {
                Vector3 mousePos = Input.mousePosition;
                Vector3 mouseWorld = ZCamera.main.ScreenToWorldPoint(mousePos);

                mouseWorld.z = 0;
                return mouseWorld;
            }
        }
    }
}