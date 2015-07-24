using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts
{
    public class LevelSequencer : MonoBehaviour
    {
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
                CurrentGame.SetForLevelStart(Area, Stage, DurationInSeconds, RequiredGems);
            }
        }

        private void OnLevelWasLoaded()
        {
            CurrentGame.SetForLevelStart(Area, Stage, DurationInSeconds, RequiredGems);
        }

        private void Update()
        {
            CurrentGame.GameData.TimeRemaining -= Time.deltaTime;
        }
    }
}