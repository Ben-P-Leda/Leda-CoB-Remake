using UnityEngine;

using Gameplay.Shared.Scripts.Player;

namespace Gameplay.Boss.Scripts.Player
{
    public class BossPlayerController : InputDrivenPlayer
    {
        private BossPlayerSequencer _sequenceController;

        public bool PushScrollActive { private get; set; }

        protected override void Awake()
        {
            base.Awake();

            _sequenceController = _transform.parent.GetComponent<BossPlayerSequencer>();
            SequenceController = _sequenceController;

            PushScrollActive = true;
        }

        protected override int GetDirection()
        {
            int direction = base.GetDirection();

            if (PushScrollActive) { direction += 1; }

            return direction;
        }

        protected override bool FacingDirectionHasChanged(int movementDirection)
        {
            return ((base.FacingDirectionHasChanged(movementDirection)) && (!PushScrollActive));
        }
    }
}
