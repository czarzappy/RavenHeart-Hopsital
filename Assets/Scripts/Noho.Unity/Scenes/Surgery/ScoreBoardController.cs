using System;
using Noho.Unity.Messages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZEngine.Unity.Core.Components;
using ZEngine.Unity.Core.Messaging;

namespace Noho.Unity.Scenes.Surgery
{
    public class ScoreBoardController : AIDU
    {
        public TMP_Text TotalScoreText;
        public TMP_Text ScoreText;
        public TMP_Text ChainBonusText;
        public Slider TimeSlider;

        public Gradient ChainBonusGradient;
        public float ChainBonusGradientCap = 10f;

        private bool mIsUpdating = true;

        public override void Init()
        {
            ZBug.Info("SCORE", "Init");
            MsgMgr.Instance.SubscribeTo<EarnScoreMsg>(OnEarnScore);

            // ResetChain();
            
            UpdateScoreboard();
            UpdateTotalScoreUI();
            UpdateChainBonusUI();
        }

        public override void UnInit()
        {
            MsgMgr.Instance.UnsubscribeFrom<EarnScoreMsg>(OnEarnScore);
        }
        private void OnEarnScore(EarnScoreMsg message)
        {
            ZBug.Info("SCORE", "On New Earn");

            BrainMain.Instance.Context.OperationManager.Score.EarnScore(message.ScoreDelta);

            UpdateScoreboard();
            UpdateChainBonusUI();
        }

        private void ResetChain()
        {
            BrainMain.Instance.Context.OperationManager.Score.ResetChain();

            UpdateScoreboard();
            UpdateTotalScoreUI();
            UpdateChainBonusUI();
        }

        public void UpdateTotalScoreUI()
        {
            TotalScoreText.text = $"{BrainMain.Instance.Context.OperationManager.Score.TotalScore:N0}";
        }
        public void Update()
        {
            if (!mIsUpdating)
            {
                return;
            }
            
            float t = BrainMain.Instance.Context.OperationManager.Score.ChainDurationT;
            TimeSlider.value = t;

            if (t <= 0)
            {
                ResetChain();
            }
        }

        private void UpdateScoreboard()
        {
            ScoreText.text = $"{BrainMain.Instance.Context.OperationManager.Score.RunningScore:N0}";
        }

        private void UpdateChainBonusUI()
        {
            // int roundChainBonus = Mathf.FloorToInt(ChainBonus);
            ChainBonusText.text = $"X{BrainMain.Instance.Context.OperationManager.Score.ChainBonus:N1}";
            ChainBonusText.color = ChainBonusGradient.Evaluate(BrainMain.Instance.Context.OperationManager.Score.ChainBonus / ChainBonusGradientCap);
        }
    }
}