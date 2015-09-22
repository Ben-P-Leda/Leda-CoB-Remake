using UnityEngine;

using Gameplay.Shared.Scripts;
using Gameplay.Boss.Scripts.Player;
using Gameplay.Boss.Scripts.Boss_Behaviour;

namespace Gameplay.Boss.Scripts
{
    public class BossLevelSequencer : LevelSequencer
    {
        private BossPlayerSequencer _playerSequencer;
        private BossBehaviour _bossController;
        private GameObject _bossBattleSequencer;

        public GameObject Boss;
        public float BossBattleStartLine;

        protected override void Awake()
        {
            base.Awake();

            _playerSequencer = PlayerSequencer.GetComponent<BossPlayerSequencer>();
            PlayerSequencerController = _playerSequencer;

            _bossController = Boss.GetComponent<BossBehaviour>();
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

            if ((_bossController.ActiveBehaviour == BossBehaviour.Behaviour.WaitingForPlayer) && (_playerSequencer.PushScrollPosition >= BossBattleStartLine - Boss_Wake_Up_Offset))
            {
                _bossController.Activate();
            }

            if (_playerSequencer.PushScrollPosition >= BossBattleStartLine)
            {
                _playerSequencer.StartBossBattle();
                _bossBattleSequencer.SetActive(true);
            }
        }

        private const float Boss_Wake_Up_Offset = 4.0f;
    }
}
