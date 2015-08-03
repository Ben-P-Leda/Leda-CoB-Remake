using UnityEngine;

using Gameplay.Shared.Scripts.Generic;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class Type08Movement : GenericBouncingMovementController, ICanBeFrozen
    {
        private SpriteRenderer _renderer;

        public bool Frozen { get; set; }

        public Sprite CompressedSprite;
        public Sprite ExpandedSprite;

        protected override void Awake()
        {
            base.Awake();

            _renderer = transform.GetComponent<SpriteRenderer>();
            _renderer.sprite = CompressedSprite;
        }

        protected override void Update()
        {
            if (!Frozen) { base.Update(); }
        }

        protected override void HandleGroundImpact()
        {
            if (Velocity.y < 0.0f)
            {
                _renderer.sprite = CompressedSprite;
                Velocity = Vector2.zero;
            }
            else
            {
                _renderer.sprite = ExpandedSprite;
                base.HandleGroundImpact();
            }
        }

        private const float Bounce_Plain_Tolerance = 0.05f;
        private const int Maximum_Bounce_Delay_Milliseconds = 3000;
    }
}
