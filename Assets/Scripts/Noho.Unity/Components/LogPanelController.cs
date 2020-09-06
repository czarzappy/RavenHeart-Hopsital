using Noho.Unity.Messages;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;
using ZEngine.Unity.Core.Models;
using Object = UnityEngine.Object;

namespace Noho.Unity.Components
{
    public class LogPanelController : UIMonoBehaviour
    {
        public LogDialogueController Example;

        public Image Header;

        public CanvasGroup MainUI;

        public RectTransform Content;
        public RectTransform ScrollRect;

        public CanvasGroup CanvasGroup;

        public Canvas Canvas;


        public StripeShaderProps StripeShaderProps;
        
        public void Awake()
        {
            Init();
        }

        public bool StartShowing;
        public void Init()
        {
            // Header.materialForRendering
            StripeShaderProps.UpdateMat(Header.material);
            
            MsgMgr.Instance.SubscribeTo<DialogueDisplayedMsg>(OnDialogueDisplayed);

            if (StartShowing)
            {
                ShowUI();
            }
            else
            {
                HideUI();
            }
        }

        private void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<DialogueDisplayedMsg>(OnDialogueDisplayed);
        }

        public void OnDestroy()
        {
            UnInit();
        }

        public void Update()
        {
            HideIfClickedOutside();
        }
        
        private void HideIfClickedOutside() {

            if (!Input.GetMouseButton(0))
            {
                return;
            }

            if (!Showing)
            {
                return;
            }
            
            
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                    rectTransform, 
                    Input.mousePosition, 
                    ZCamera.main))
            {
                HideUI();
            }
        }

        public void ShowUI()
        {
            // this.Show();
            CanvasGroup.alpha = 1f;
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;

            if (MainUI != null)
            {
                MainUI.blocksRaycasts = false;
            }
            
            Showing = true;

        }

        public void HideUI()
        {
            // this.Hide();
            CanvasGroup.alpha = 0f;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;

            if (MainUI != null)
            {
                MainUI.blocksRaycasts = true;
            }
            
            Showing = false;
        }

        public bool Showing = false;

        public void Toggle()
        {
            if (Showing)
            {
                HideUI();
            }
            else
            {
                ShowUI();
            }
        }

        private void OnDialogueDisplayed(DialogueDisplayedMsg message)
        {
            var log = Object.Instantiate(Example, Content, false);

            Content.ScaleToChildren();
            
            log.Show();
            log.Init(message.Speaker, message.Dialogue, message.Color);
            
            Canvas.ForceUpdateCanvases();

            // float targetY = RectExtensions.GetTargetYToMatchBottom(Content.rect, ScrollRect.rect);

            // var currPos = Content.anchoredPosition;

            // currPos.y = targetY;

            // Content.anchoredPosition = currPos;
        }
    }
}