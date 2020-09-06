using Noho.Unity.Messages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Title
{
    public class SpriteInfoScrollItem : ScrollItem<Sprite>
    {
        public Button CopyToClipboardBtn;

        public TMP_Text Text;
        public Image Image;

        private string mAssetName;
        public override void Init(Sprite sprite)
        {
            mAssetName = sprite.name;
            
            Text.text = mAssetName;
            Image.sprite = sprite;
            
            CopyToClipboardBtn.onClick.AddListener(OnCopyToClipboardClick);
        }

        private void OnCopyToClipboardClick()
        {
            mAssetName.CopyToClipboard();
            Send.Msg(new NewNotifMsg
            {
                Notif = $"Copied \"{mAssetName}\" to clipboard!"
            });
        }
    }
}