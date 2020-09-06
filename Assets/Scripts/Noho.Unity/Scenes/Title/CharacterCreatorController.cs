using Noho.Models;
using TMPro;
using UnityEngine;
using ZEngine.Unity.Core.Components;

namespace Noho.Unity.Scenes.Title
{
    public class CharacterCreatorController : UIMonoBehaviour
    {
        public TMP_InputField NameInputField;
        
        // public List<>

        public void Awake()
        {
            Init();
        }

        public void Init()
        {
            NameInputField.onEndEdit.AddListener(OnEndEdit);

            NameInputField.text = PlayerPrefs.GetString(PlayerPrefKeys.PLAYER_NAME, "You");
        }

        private void OnEndEdit(string newName)
        {
            PlayerPrefs.SetString(PlayerPrefKeys.PLAYER_NAME, newName);
            PlayerPrefs.Save();
        }
    }
}