using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts
{
    public class LevelSequencer : MonoBehaviour
    {
        private LevelState _levelState;

        public int Area;
        public AreaStage Stage;
        public float DurationInSeconds;
        public int RequiredGems;

        public bool DebuggingLevel;

        private void Awake()
        {
            if (DebuggingLevel)
            {
                CurrentGame.SetForNewGame();
                SetForLevelStart();
            }
        }

        private void OnLevelWasLoaded()
        {
            
        }

        private void SetForLevelStart()
        {
            _levelState = LevelState.GetReady;
            CurrentGame.SetForLevelStart(Area, Stage, DurationInSeconds, RequiredGems);
            //if (Stage != AreaStage.Bonus) { Time.timeScale = 0.0f; }
        }

        private void Update()
        {
            switch (_levelState)
            {
                case LevelState.GetReady: UpdateForGetReady(); break;
                case LevelState.InPlay: UpdateForInPlay(); break;
            }
        }

        private void UpdateForGetReady()
        {

        }

        private void UpdateForInPlay()
        {
            CurrentGame.GameData.TimeRemaining -= Time.deltaTime;
        }

        private enum LevelState
        {
            GetReady,
            InPlay
        }
    }
}