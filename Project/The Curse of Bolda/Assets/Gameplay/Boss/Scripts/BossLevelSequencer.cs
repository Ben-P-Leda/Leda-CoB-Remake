using UnityEngine;

using Gameplay.Shared.Scripts;
using Gameplay.Boss.Scripts.Player;

namespace Gameplay.Boss.Scripts
{
    public class BossLevelSequencer : LevelSequencer
    {
        private BossPlayerSequencer _playerSequencer;
        private GameObject _bossBattleSequencer;

        public float BossBattleStartLine;

        protected override void Awake()
        {
            base.Awake();

            _playerSequencer = PlayerSequencer.GetComponent<BossPlayerSequencer>();
            PlayerSequencerController = _playerSequencer;

            _bossBattleSequencer = transform.FindChild("Boss Battle Sequencer").gameObject;
        }

        protected override void StartGameplay()
        {
            _playerSequencer.ActivatePushScrolling();

            base.StartGameplay();
        }

        protected override void UpdateForInPlay()
        {
            base.UpdateForInPlay();

            if (_playerSequencer.PushScrollPosition >= BossBattleStartLine)
            {
                _playerSequencer.StartBossBattle();
                _bossBattleSequencer.SetActive(true);
            }
        }
    }
}
