using UnityEngine;

using Shared.Scripts;
using Gameplay.Shared.Scripts.Player;
using Gameplay.Shared.Scripts.Enemy_Behaviours;

namespace Gameplay.Shared.Scripts
{
    public class LevelSequencer : MonoBehaviour
    {
        private LevelState _levelState;
        private FadeTransitioner _fadeTransitioner;
        private PlayerSequencer _playerSequencer;
        private ICanBeFrozen[] _freezableEnemyScripts;

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

            _freezableEnemyScripts = new ICanBeFrozen[Enemies.transform.childCount];
            for (int i=0; i<_freezableEnemyScripts.Length; i++)
            {
                _freezableEnemyScripts[i] = Enemies.transform.GetChild(i).GetComponent<ICanBeFrozen>();
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
            _levelState = LevelState.GetReady;

            if (CurrentGame.GameData.TimeRemaining <= 0.0f) { CurrentGame.GameData.TimeRemaining = DurationInSeconds; }
            CurrentGame.RestorePlayerEnergy();

            SetEnemiesFreezeState(true);

            _fadeTransitioner.FadeIn();
        }

        private void SetEnemiesFreezeState(bool freeze)
        {
            for (int i = 0; i < _freezableEnemyScripts.Length; i++) { _freezableEnemyScripts[i].Frozen = freeze; }
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
                _levelState = LevelState.InPlay;
                SetEnemiesFreezeState(false);
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
            SetEnemiesFreezeState(true);
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

        private enum LevelState
        {
            GetReady,
            InPlay,
            SequenceRunning
        }
    }
}