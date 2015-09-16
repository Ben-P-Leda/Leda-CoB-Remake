using UnityEngine;

using Shared.Scripts;

using Gameplay.Shared.Scripts.Player;

namespace Gameplay.Boss.Scripts.Player
{
    public class BossPlayerController : InputDrivenPlayer
    {
        private BossPlayerSequencer _sequenceController;
        private bool _beingPushed;

        public bool PushScrollActive { private get; set; }
        public bool Obstructed { private get; set; }

        protected override void Awake()
        {
            base.Awake();

            _sequenceController = _transform.parent.GetComponent<BossPlayerSequencer>();
            SequenceController = _sequenceController;
        }

        protected override void Reset()
        {
            base.Reset();

            _beingPushed = false;

            Obstructed = false;
        }

        protected override void Update()
        {
            base.Update();

            if ((_beingPushed) && (Obstructed))
            {
                CurrentGame.GameData.Energy = 0;
            }
        }

        protected override int GetDirection()
        {
            int direction = base.GetDirection();

            if (PushScrollActive) { direction += 1; }
            if ((_beingPushed) && (direction < 1)) { direction = 1; }

            return direction;
        }

        protected override bool FacingDirectionHasChanged(int movementDirection)
        {
            return ((base.FacingDirectionHasChanged(movementDirection)) && (!PushScrollActive));
        }

        protected override void Flip()
        {
            if (!PushScrollActive) { base.Flip(); }
        }

        protected override void SetWalkingAnimationFlag(int direction)
        {
            if (Obstructed) { direction = 0; }

            base.SetWalkingAnimationFlag(direction);
        }

        protected override void OnTriggerEnter2D(Collider2D collider)
        {
            base.OnTriggerEnter2D(collider);

            if (collider.tag == "Push Collider") { _beingPushed = true; }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.tag == "Push Collider") { _beingPushed = false; }
        }
    }
}
