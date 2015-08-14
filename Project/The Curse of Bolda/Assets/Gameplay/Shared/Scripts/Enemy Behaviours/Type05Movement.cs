using UnityEngine;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class Type05Movement : HorizontalMovementBetweenLimits
    {
        private float _centerLineY;
        private float _offsetRotator;

        protected override void Start()
        {
            base.Start();

            _centerLineY = Position.y;
            _offsetRotator = 0.0f;
        }

        protected override void Update()
        {
            base.Update();

            if (!Frozen)
            {
                _offsetRotator = (_offsetRotator + (Time.deltaTime * Offset_Rotator_Step_Modifier)) % 360.0f;
                Position = new Vector2(Position.x, _centerLineY + (Mathf.Sin(_offsetRotator) * Wave_Maximum_Offset));
            }
        }

        private const float Offset_Rotator_Step_Modifier = 4.0f;
        private const float Wave_Maximum_Offset = 0.35f;
    }
}