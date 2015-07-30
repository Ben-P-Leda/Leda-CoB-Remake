using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Collectables
{
    public class BouncingCollectableBase : CollectableBase
    {
        private Transform _transform;
        private Rigidbody2D _rigidBody2D;
        private float _bouncePlainY;

        private void Awake()
        {
            _transform = transform;
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _bouncePlainY = transform.position.y;
        }

        private void Update()
        {
            if (_transform.position.y <= _bouncePlainY + Bounce_Plain_Tolerance) { _rigidBody2D.velocity = new Vector2(0.0f, 5.0f); }
        }

        protected override void HandlePlayerContact()
        {
            CurrentGame.RestorePlayerEnergy();
            base.HandlePlayerContact();
        }

        private const float Bounce_Plain_Tolerance = 0.01f;
    }
}
