using UnityEngine;

using Gameplay.Shared.Scripts;
using Gameplay.Normal.Scripts.Player;

namespace Gameplay.Normal.Scripts
{
    public class NormalLevelSequencer : LevelSequencer
    {
        private NormalPlayerSequencer _playerSequencer;

        protected override void Awake()
        {
            base.Awake();

            _playerSequencer = PlayerSequencer.GetComponent<NormalPlayerSequencer>();
            PlayerSequencerController = _playerSequencer;
        }
    }
}
