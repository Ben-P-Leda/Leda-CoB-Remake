using UnityEngine;

using Gameplay.Shared.Scripts.Player;

namespace Gameplay.Boss.Scripts.Player
{
    public class BossPlayerController : InputDrivenPlayer
    {
        private BossPlayerSequencer _sequenceController;

        protected override void Awake()
        {
            base.Awake();

            _sequenceController = _transform.parent.GetComponent<BossPlayerSequencer>();
            SequenceController = _sequenceController;
        }
    }
}
