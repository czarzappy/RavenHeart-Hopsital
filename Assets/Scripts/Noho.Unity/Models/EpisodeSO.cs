using Noho.Configs;
using UnityEngine;
using UnityEngine.UI;

namespace Noho.Unity.Models
{
    [CreateAssetMenu(fileName = "Palette", menuName = "ScriptableObjects/Noho/EpisodeSO", order = 1)]
    public class EpisodeSO : ScriptableObject
    {
        public Image PreviewImage;
        public TextAsset Text;
    }
}