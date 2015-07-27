using UnityEngine;

using Shared.Scripts;
using Gameplay.Shared.Scripts.Player;

namespace Gameplay.Shared.Scripts
{
    public class LevelSequencer : MonoBehaviour
    {
        private LevelState _levelState;
        private FadeTransitioner _fadeTransitioner;
        private PlayerSequencer _playerSequencer;

        public int Area;
        public AreaStage Stage;
        public float DurationInSeconds;
        public int RequiredGems;
        public Vector2 PlayerStartPosition;

        public bool DebuggingLevel;

        public GameObject PlayerSequencer;

        private void Awake()
        {
            _fadeTransitioner = GetComponent<FadeTransitioner>();
            _fadeTransitioner.Timer = GetComponent<IndependentTimer>();
            _playerSequencer = PlayerSequencer.GetComponent<PlayerSequencer>();

            if (DebuggingLevel)
            {
                CurrentGame.SetForNewGame();
                SetForLevelStart();
            }
        }

        private void OnLevelWasLoaded()
        {
            SetForLevelStart();
        }

        private void SetForLevelStart()
        {
            CurrentGame.SetForLevelStart(Area, Stage, RequiredGems, PlayerStartPosition);

            if (Stage != AreaStage.Bonus) { SetForNewLife(); }
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
            if (Input.anyKeyDown)
            {
                Time.timeScale = 1.0f;
                _levelState = LevelState.InPlay;
            }
        }

        private void UpdateForInPlay()
        {
            CurrentGame.GameData.TimeRemaining = Mathf.Max(CurrentGame.GameData.TimeRemaining - Time.deltaTime, 0.0f);

            if (CurrentGame.GameData.Energy <= 0.0f) { HandlePlayerDeath(); }
            if (CurrentGame.GameData.TimeRemaining <= 0.0f) { HandlePlayerDeath(); }
        }

        private void HandlePlayerDeath()
        {
            CurrentGame.GameData.Lives -= 1;

            if (CurrentGame.GameData.Lives > 0)
            {
                _levelState = LevelState.SequenceRunning;

                _fadeTransitioner.TransitionCompletionHandler = SetForNewLife;
                _playerSequencer.SequenceCompleteHandler = _fadeTransitioner.FadeOut;
                _playerSequencer.StartDeathSequence(PlayerDeathSequence.Generic);
            }
            else
            {
                // TODO: Game over
            }
        }

        private void SetForNewLife()
        {
            _playerSequencer.StartNewLife();
            _levelState = LevelState.GetReady;

            Time.timeScale = 0.0f;

            if (CurrentGame.GameData.TimeRemaining <= 0.0f) { CurrentGame.GameData.TimeRemaining = DurationInSeconds; }
            CurrentGame.SetForNewLife();
            
            _fadeTransitioner.FadeIn();
        }

        private enum LevelState
        {
            GetReady,
            InPlay,
            SequenceRunning
        }
    }
}