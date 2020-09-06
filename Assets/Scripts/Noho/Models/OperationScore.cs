using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Noho.Models
{
    public class OperationScore
    {
        private const float CHAIN_BONUS_RATE = 0.5f;
        private const float START_CHAIN_DURATION_SECONDS = 4f;
        private const float START_CHAIN_DISPLAY_DELAY_SECONDS = 1f;
        
        private readonly List<int> mLastScores = new List<int>();
        
        public float TotalScore;
        public float ChainBonus = 1f;
        public float MinChainDuration = 1f;

        private float mLastScoreTime;
        private float mUpdateTime;
        private bool mFirstScore = true;

        #region Properties

        public float FinalScore
        {
            get
            {
                return TotalScore + RunningScore;
            }
        }
        private float ChainElapsedTime => mUpdateTime - mLastScoreTime;
        public float RunningScore => mLastScores.Sum() * ChainBonus;
        private float ChainDurationSeconds => START_CHAIN_DURATION_SECONDS;
        public float ChainDurationT
        {
            get
            {
                // Want the slider to go down with time
                // first second will appear as fully loaded
                float t = (ChainElapsedTime - START_CHAIN_DISPLAY_DELAY_SECONDS) / ChainDurationSeconds;
                t = Mathf.Clamp(t, 0f, 1f);
                return 1f - t;
            }
        }
        
        #endregion

        public void Tick(float deltaTime)
        {
            mUpdateTime += deltaTime;
        }

        public void EarnScore(int scoreDelta)
        {
            mLastScoreTime = Time.time;
            mUpdateTime = mLastScoreTime;
            mLastScores.Add(scoreDelta);

            if (!mFirstScore)
            {
                ChainBonus += CHAIN_BONUS_RATE;
            }
            else
            {
                mFirstScore = false;
            }
        }

        public void ResetChain()
        {
            TotalScore += RunningScore;
            
            mFirstScore = true;
            ChainBonus = 1;
            mLastScores.Clear();

            mLastScoreTime = -ChainDurationSeconds;
            mUpdateTime = 0;
        }

        public void FullReset()
        {
            TotalScore = 0;
            
            mFirstScore = true;
            ChainBonus = 1;
            mLastScores.Clear();

            mLastScoreTime = -ChainDurationSeconds;
            mUpdateTime = 0;
        }
    }
}