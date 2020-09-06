using System.Collections;
using Noho.Configs;
using Noho.Models;
using Noho.Unity.Messages;
using Noho.Unity.Models;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Extensions;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Components
{
    public class CharacterController : UIMonoBehaviour
    {
        public Image CharacterImage;
        public Canvas Canvas;
        public bool ShouldFlip;
        public int StagePosIndex;
        public string CharacterDevName;
        private Vector2 mCharacterIdlePos = Vector2.zero;
        public Vector2 CharacterIdlePos => mCharacterIdlePos;
        public bool IsInit = false;

        public ParticleSystem Love;

        public override string ToString()
        {
            return $"{CharacterDevName} (Pos: {mCharacterIdlePos}) [IDX: {StagePosIndex}]";
        }

        public void Awake()
        {
            if (Love != null)
            {
                ZBug.Info("Character", $"{CharacterDevName} awake - turning love off");
                Love.Stop();
            }
            
            ZBug.Info("Character", $"Awake! ShouldFlip: {ShouldFlip}");
            if (ShouldFlip)
            {
                transform.FlipX();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        private void OnEnable()
        {
            Init();
        }

        public void OnDestroy()
        {
            UnInit();
        }

        private void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<CharacterLoveChangedMsg>(OnCharacterLoveChanged);
        }

        public IEnumerator ShowLove()
        {
            ZBug.Info("Character", "Showing Love");
            Love.Play();
            
            yield return new WaitForSeconds(NohoUISettings.LOVE_DISPLAY_DURATION);
            
            ZBug.Info("Character", "Stopping Love");
            Love.Stop();
        }
        

        private void OnCharacterLoveChanged(CharacterLoveChangedMsg message)
        {
            if (message.CharacterDevName != CharacterDevName)
            {
                return;
            }
            ZBug.Info("Character", $"{CharacterDevName} Handling love");

            if (message.LoveDelta > 0)
            {
                // StartCoroutine(nameof(ShowLove));
                StartCoroutine(this.ShowLove());
            }
        }
        public void Init()
        {
            ZBug.Info("Character", $"Init!");
            if (Love != null)
            {
                Love.Stop();
            }

            // only initialize this value once
            if (mCharacterIdlePos == Vector2.zero)
            {
                mCharacterIdlePos = rectTransform.anchoredPosition;
            }
            
            IsInit = true;
            
            MsgMgr.Instance.SubscribeTo<CharacterLoveChangedMsg>(OnCharacterLoveChanged);
        }

        public void ResetCharacter(int stagePosIdx)
        {
            StagePosIndex = stagePosIdx;
            CharacterDevName = null;
            CharacterConfig = null;
            CharacterImage.sprite = null;
        }

        // Update is called once per frame
        void Update()
        {
        }


        public CharacterConfig CharacterConfig;
        public void SetCharacter(string characterDevName)
        {
            // ConfigIdResolver.Instance.Resolve();
            SetCharacterDevName(characterDevName);

            if (characterDevName == NohoParsingConstants.CHARACTER_DEV_NAME_PROTAG)
            {
                var selection = App.Instance.RuntimeData.ProtagSelection;

                characterDevName += $"-{selection}";
            }
            
            // Show character image
            CharacterConfig = NohoConfigResolver.GetConfig<CharacterConfig>(characterDevName);

            string[] path;
            if (CharacterConfig == null)
            {
                path = null;
            }
            else
            {
                path = CharacterConfig.DefaultSpritePath;
            }

            CharacterImage.sprite = ZSource.Load<Sprite>(path);

            if (Canvas != null)
            {
                Canvas.sortingOrder = (int) CharacterConfig.Size;
            }
            
            CharacterImage.SetNativeSize();
        }

        public void SetCharacterDevName(string characterDevName)
        {
            CharacterDevName = characterDevName;
        }

        public void SetExpression(string expressionDevName)
        {
            string[] getPoseSprite = CharacterConfig.GetPoseSpritePath(expressionDevName);
        
            CharacterImage.sprite = ZSource.Load<Sprite>(getPoseSprite);
        }
    }
}
