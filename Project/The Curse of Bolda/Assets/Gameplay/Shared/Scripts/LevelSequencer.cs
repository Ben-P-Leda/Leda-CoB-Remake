using UnityEngine;
using System.Collections.Generic;
using Shared.Scripts;
using Gameplay.Shared.Scripts.Generic;
using Gameplay.Shared.Scripts.Player;
using Gameplay.Shared.Scripts.Enemy_Behaviours;
using Gameplay.Shared.Scripts.Effects;

namespace Gameplay.Shared.Scripts
{
    public class LevelSequencer : MonoBehaviour
    {
        private FadeTransitioner _fadeTransitioner;
        protected IPlayerSequencer PlayerSequencerController { private get; set; }
        private List<ICanBeFrozen> _freezableEnemyScripts;
        private List<IChangesStateOnNewLifeStart> _objectsToUpdateOnNewLifeStart;
        private GameObject _getReadySequencer;
        private GameObject _endLevelSequencer;
        private LevelClearChrome _endLevelSequencerScript;
        private GameObject _gameOverSequencer;
        private float _timeSinceGameOver;

        public int Area;
        public AreaStage Stage;
        public float DurationInSeconds;
        public int RequiredGems;
        public Vector2 PlayerStartPosition;
        public bool PlayerStartFacingLeft;
        public string NextLevel;

        public bool DebuggingLevel;

        public GameObject PlayerSequencer;
        public GameObject Enemies;
        public GameObject Switches;

        protected virtual void Awake()
        {
            _fadeTransitioner = GetComponent<FadeTransitioner>();

            _getReadySequencer = transform.FindChild("Get Ready Sequencer").gameObject;
            _endLevelSequencer = transform.FindChild("End Level Sequencer").gameObject;
            _endLevelSequencerScript = _endLevelSequencer.GetComponent<LevelClearChrome>();
            _gameOverSequencer = transform.FindChild("Game Over Sequencer").gameObject;

            _objectsToUpdateOnNewLifeStart = new List<IChangesStateOnNewLifeStart>();
            _freezableEnemyScripts = new List<ICanBeFrozen>();
            for (int i = 0; i < Enemies.transform.childCount; i++)
            {
                ICanBeFrozen freezableScript = Enemies.transform.GetChild(i).GetComponent<ICanBeFrozen>();
                if (freezableScript != null) { _freezableEnemyScripts.Add(freezableScript); }

                IChangesStateOnNewLifeStart newLifeScript = Enemies.transform.GetChild(i).GetComponent<IChangesStateOnNewLifeStart>();
                if (newLifeScript != null) { _objectsToUpdateOnNewLifeStart.Add(newLifeScript); }
            }

            for (int i=0; i < Switches.transform.childCount; i++)
            {
                IChangesStateOnNewLifeStart newLifeScript = Switches.transform.GetChild(i).GetComponent<IChangesStateOnNewLifeStart>();
                if (newLifeScript != null) { _objectsToUpdateOnNewLifeStart.Add(newLifeScript); }
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
            PlayerSequencerController.StartNewLife();

            CurrentGame.SetForNewLife();
            for (int i = 0; i < _objectsToUpdateOnNewLifeStart.Count; i++) { _objectsToUpdateOnNewLifeStart[i].SetForPlayerNewLifeStart(); }

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
                StartGameplay();
            }
        }

        protected virtual void StartGameplay()
        {
            CurrentGame.StartGameplay();
            SetEnemiesFreezeState(false);
        }

        private void UpdateForInPlay()
        {
            CurrentGame.UpdateTimers(Time.deltaTime);

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
            PlayerSequencerController.StartDeathSequence(PlayerDeathSequence.Generic, HandleLifeLossSequenceComplete);
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
            else if ((_endLevelSequencerScript.BonusCountComplete) && (Input.anyKeyDown))
            {
                _fadeTransitioner.TransitionCompletionHandler = StartNextLevel;
                _fadeTransitioner.FadeOut();

                CurrentGame.GameData.GameplayState = GameplayState.CueNextLevel;
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

        private void StartNextLevel()
        {
            Application.LoadLevel(NextLevel);
        }

        private const float Game_Over_State_Duration = 5.0f;
    }
}