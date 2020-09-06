using UnityEngine;
using ZEngine.Unity.Core.Extensions;

namespace Noho.Unity.Scenes.Surgery.Gunk
{
    public static class GunkUtil
    {
        public static GameObject BandageCloneAndShow(GameObject otherGameObject, Transform transform)
        {
            var bandage = Object.Instantiate(otherGameObject, transform, true);
            bandage.Show();

            return bandage;
        }
    }
}