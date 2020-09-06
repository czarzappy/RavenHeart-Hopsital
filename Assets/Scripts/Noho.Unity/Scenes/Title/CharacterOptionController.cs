using Noho.Configs;
using Noho.Unity.Models;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Extensions;

namespace Noho.Unity.Scenes.Title
{
    public class CharacterOptionController : MonoBehaviour
    {
        public Image SidebarImage;

        public Image BottomPanelImage;

        public Image CharacterImage;

        public TMP_Text CharacterNameText;
        public TMP_Text RoleText;
        public TMP_Text AffectionText;

        // public LocalizedString CharacterNameLoc;
        // public LocalizedString RoleLoc;

        public Button CharacterSelect;
        

        public void Init(CharacterConfig characterConfig, bool isAvailable, UnityAction onCharacterSelected)
        {
            // CharacterName.
            // CharacterName.text = characterConfig.Name;
            // RoleText.text = characterConfig.Role;

            // CharacterNameLoc.SetCharacterName(characterConfig.DevName);
            // RoleLoc.SetReference(LocTables.CHARACTER_TEXT, $"CHARACTER_ROLE_{characterConfig.DevName}");
            RoleText.text = characterConfig.RawRole;
            CharacterNameText.text = characterConfig.RawName;

            // CharacterNameLoc.RegisterChangeHandler(OnCharacterNameChange);
            // RoleLoc.RegisterChangeHandler(OnRoleTextChange);
            
            // LocalizationSettings.
            // var reference = new TableEntryReference();
            // reference.Key = $"CHARACTER_NAME_{characterConfig}";
            // CharacterName.SetReference();
            
            Color color = characterConfig.CharacterNameBackgroundColor.ToUnity();

            SidebarImage.color = color;
            BottomPanelImage.color = color;

            CharacterImage.sprite = ZSource.Load<Sprite>(characterConfig.DefaultSpritePath);

            CharacterImage.color = (isAvailable) ? Color.white : Color.black;

            CharacterSelect.interactable = isAvailable;

            if (isAvailable)
            {
                CharacterSelect.onClick.AddListener(onCharacterSelected);
            }

            int love = App.Instance.PersistentData.GetCurrentLoveBy(characterConfig.DevName);

            string sign = love >= 0 ? "+" : "-";
            AffectionText.text = $"Affection: {sign}{love}";
        }

        private void OnRoleTextChange(string value)
        {
            RoleText.text = value;
        }

        private void OnCharacterNameChange(string value)
        {
            CharacterNameText.text = value;
        }

        private void OnCharacterSelected()
        {
            ZBug.Info("Character Selected!");
        }
    }
}
