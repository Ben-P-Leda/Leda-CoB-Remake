using UnityEngine;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class Type12Movement : HorizontalMovementBetweenLimits
    {
        private Transform _playerTransform;
        private Rect _activationArea;

        public GameObject Player;

        protected override void Awake()
        {
            base.Awake();

            _playerTransform = Player.transform;
            _activationArea = new Rect(
                Position.x - LeftSideLimitOffset,
                Position.y - Vertical_Margin,
                RightSideLimitOffset + LeftSideLimitOffset,
                Vertical_Margin * 2.0f);
        }

        protected override void Update()
        {
            if ((_activationArea.Contains(_playerTransform.position)) && (!Frozen))
            {
                ApplyAcceleration((Acceleration * Mathf.Sign(_playerTransform.position.x - Position.x)));
            }
            else if (HorizontalSpeed != 0.0f)
            {
                ApplyAcceleration(-HorizontalSpeed);
            }
        }

        private const float Vertical_Margin = 0.45f;
    }
}