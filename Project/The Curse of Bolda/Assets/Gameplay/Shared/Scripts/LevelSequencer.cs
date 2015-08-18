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
        private GameObject _levelClearSequencer;

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

            _levelClearSequencer = transform.FindChild("End Level Sequencer").gameObject;

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
            CurrentGame.SetForLevelStart(Area, Stage, RequiredGems, PlayerStartPosition, PlayerStartFacingLeft);

            if (Stage != AreaStage.Bonus) { SetForNewLife(); }
        }

        private void SetForNewLife()
        {
            _playerSequencer.StartNewLife();

            if (CurrentGame.GameData.TimeRemaining <= 0.0f) { CurrentGame.GameData.TimeRemaining = DurationInSeconds; }
            CurrentGame.SetForNewLife();

            SetEnemiesFreezeState(true);

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
                // TODO: Game over
            }
        }

        private void UpdateForLevelCleared()
        {
            if (!_levelClearSequencer.activeInHierarchy)
            {
                _levelClearSequencer.SetActive(true);
            }
        }
    }
}