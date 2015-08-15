using UnityEngine;

using Gameplay.Shared.Scripts.Player;
using Gameplay.Shared.Scripts.Shots;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class Type05Movement : HorizontalMovementBetweenLimits
    {
        private float _centerLineY;
        private float _offsetRotator;
        private EnemyBomb _bomb;

        public float DetectionRange;
        public GameObject Bomb;

        protected override void Start()
        {
            base.Start();

            _bomb = Bomb.GetComponent<EnemyBomb>();

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

            RaycastHit2D ray = Physics2D.Raycast(Position, Vector2.down, DetectionRange, PlayerSequencer.Player_Physics_Layer_Index);
            if ((ray.collider != null) && (ray.collider.tag == "Kev")) { AttemptBombDrop(); }
        }

        private void AttemptBombDrop()
        {
            if (!Bomb.activeInHierarchy)
            {
                Bomb.SetActive(true);
                _bomb.Activate(Position);
            }
        }

        private const float Offset_Rotator_Step_Modifier = 4.0f;
        private const float Wave_Maximum_Offset = 0.35f;
    }
}