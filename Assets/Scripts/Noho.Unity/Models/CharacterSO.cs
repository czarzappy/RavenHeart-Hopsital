using Noho.Configs;
using UnityEngine;

namespace Noho.Unity.Models
{
    [CreateAssetMenu(fileName = "Palette", menuName = "ScriptableObjects/Noho/CharacterSO", order = 1)]
    public class CharacterSO : ScriptableObject
    {
        public CharacterConfig Config;
    }
}