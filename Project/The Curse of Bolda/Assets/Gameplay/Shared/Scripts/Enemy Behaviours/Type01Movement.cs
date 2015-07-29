using UnityEngine;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class Type01Movement : HorizontalMovementBetweenLimits
    {
        private Animator _animator;

        protected override void Awake()
        {
            base.Awake();

            _animator = GetComponent<Animator>();
        }

        public override void SetFrozen(bool freeze)
        {
            base.SetFrozen(freeze);

            _animator.enabled = !freeze;
        }
    }
}