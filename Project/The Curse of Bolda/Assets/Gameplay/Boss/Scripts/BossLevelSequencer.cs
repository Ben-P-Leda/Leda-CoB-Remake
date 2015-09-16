using UnityEngine;

using Gameplay.Shared.Scripts;
using Gameplay.Boss.Scripts.Player;

namespace Gameplay.Boss.Scripts
{
    public class BossLevelSequencer : LevelSequencer
    {
        private BossPlayerSequencer _playerSequencer;

        protected override void Awake()
        {
            base.Awake();

            _playerSequencer = PlayerSequencer.GetComponent<BossPlayerSequencer>();
            PlayerSequencerController = _playerSequencer;
        }

        protected override void StartGameplay()
        {
            base.StartGameplay();

            _playerSequencer.ActivatePushScrolling();
        }
    }
}
