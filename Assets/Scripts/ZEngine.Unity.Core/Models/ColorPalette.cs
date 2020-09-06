using UnityEngine;
using UnityEngine.Serialization;

namespace ZEngine.Unity.Core.Models
{
    
    [CreateAssetMenu(fileName = "Palette", menuName = "ScriptableObjects/ColorPalette", order = 1)]
    public class ColorPalette : ScriptableObject
    {
        public Color[] Colors;
    }
}