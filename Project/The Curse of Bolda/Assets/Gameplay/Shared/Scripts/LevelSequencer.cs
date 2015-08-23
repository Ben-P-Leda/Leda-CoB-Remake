using UnityEngine;
using System.Collections.Generic;
using Shared.Scripts;
using Gameplay.Shared.Scripts.Player;
using Gameplay.Shared.Scripts.Enemy_Behaviours;

namespace Gameplay.Shared.Scripts
{
    public class LevelSequencer : MonoBehaviour
    {
        private FadeTransitioner _fadeTransitioner;
        private PlayerSequencer _playerSequencer;
        private List<ICanBeFrozen> _freezableEnemyScripts;
        private GameObject _getReadySequencer;
        private GameObject _endLevelSequencer;
        private GameObject _gameOverSequencer;
        private float _timeSinceGameOver;

        public int Area;
        public AreaStage Stage;
        public float DurationInSeconds;
        public int RequiredGems;
        public Vector2 PlayerStartPosition;
        public bool PlayerStartFacingLeft;

        public bool DebuggingLevel;

        public GameObject PlayerSequencer;
        public GameObject Enemies;

        private void Awake()
        {
            _fadeTransitioner = GetComponent<FadeTransitioner>();
            _playerSequencer = PlayerSequencer.GetComponent<PlayerSequencer>();

            _getReadySequencer = transform.FindChild("Get Ready Sequencer").gameObject;
            _endLevelSequencer = transform.FindChild("End Level Sequencer").gameObject;
            _gameOverSequencer = transform.FindChild("Game Over Sequencer").gameObject;

            _freezableEnemyScripts = new List<ICanBeFrozen>();
            for (int i = 0; i < Enemies.transform.childCount; i++)
            {
                ICanBeFrozen freezableScript = Enemies.transform.GetChild(i).GetComponent<ICanBeFrozen>();
                if (freezableScript != null) { _freezableEnemyScripts.Add(freezableScript); }
            }
        }

        private void Start()
        {
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
            CurrentGame.SetForLevelStart(Area, Stage, DurationInSeconds, RequiredGems, PlayerStartPosition, PlayerStartFacingLeft);

            if (Stage != AreaStage.Bonus) { SetForNewLife(); }
        }

        private void SetForNewLife()
        {
            _playerSequencer.StartNewLife();

            CurrentGame.SetForNewLife();

            SetEnemiesFreezeState(true);

            _getReadySequencer.SetActive(true);
            _fadeTransitioner.FadeIn();
        }

        private void SetEnemiesFreezeState(bool freeze)
        {
            for (int i = 0; i < _freezableEnemyScripts.Count; i++) { _freezableEnemyScripts[i].Frozen = freeze; }
        }

        private void Update()
        {
            switch (CurrentGame.GameData.GameplayState)
            {
                case GameplayState.GetReady: UpdateForGetReady(); break;
                case GameplayState.InPlay: UpdateForInPlay(); break;
                case GameplayState.LevelComplete: UpdateForLevelCleared(); break;
                case GameplayState.GameOver: UpdateForGameOver(); break;
            }
        }

        private void UpdateForGetReady()
        {
            if (Input.anyKeyDown)
            {
                CurrentGame.StartGameplay();
                SetEnemiesFreezeState(false);
            }
        }

        private void UpdateForInPlay()
        {
            CurrentGame.UpdateTimer(Time.deltaTime);

            if (CurrentGame.GameData.Energy <= 0.0f) { HandlePlayerDeath(); }
            if (CurrentGame.GameData.TimeRemaining <= 0.0f) { HandlePlayerDeath(); }
        }

        private void HandlePlayerDeath()
        {
            SetEnemiesFreezeState(true);
            CurrentGame.GameData.TimerIsFrozen = true;
            CurrentGame.GameData.Lives -= 1;

            CurrentGame.GameData.GameplayState = GameplayState.SequenceRunning;

            _fadeTransitioner.TransitionCompletionHandler = SetForNewLife;
            _playerSequencer.StartDeathSequence(PlayerDeathSequence.Generic, HandleLifeLossSequenceComplete);
        }

        private void HandleLifeLossSequenceComplete()
        {
            if (CurrentGame.GameData.Lives > 0)
            {
                _fadeTransitioner.FadeOut();
            }
            else
            {
                CurrentGame.GameData.GameplayState = GameplayState.GameOver;
                _timeSinceGameOver = 0.0f;
                _gameOverSequencer.SetActive(true);
            }
        }

        private void UpdateForLevelCleared()
        {
            if (!_endLevelSequencer.activeInHierarchy)
            {
                _endLevelSequencer.SetActive(true);
            }
        }

        private void UpdateForGameOver()
        {
            if ((Input.GetKeyDown(KeyCode.RightShift)) || 
                ((_timeSinceGameOver <= Game_Over_State_Duration) && (_timeSinceGameOver + Time.deltaTime > Game_Over_State_Duration)))
            {
                _fadeTransitioner.TransitionCompletionHandler = ExitGame;
                _fadeTransitioner.FadeOut();
            }

            _timeSinceGameOver += Time.deltaTime;
        }

        private void ExitGame()
        {
            Application.LoadLevel("TitleScene");
        }

        private const float Game_Over_State_Duration = 5.0f;
    }
}