using UnityEngine;

namespace Noho.Unity.Models
{
    [CreateAssetMenu(fileName = "collection", menuName = "ScriptableObjects/Noho/ScriptCollection", order = 1)]
    public class ScriptCollection : ScriptableObject
    {
        public TextAsset[] TextAssets;
    }
}