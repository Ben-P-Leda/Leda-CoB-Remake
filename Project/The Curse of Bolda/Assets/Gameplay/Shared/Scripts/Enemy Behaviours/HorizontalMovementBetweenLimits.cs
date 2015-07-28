using UnityEngine;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class HorizontalMovementBetweenLimits : BasicBehaviour
    {
        private Transform _transform;
        private Rigidbody2D _rigidBody2D;
        private float _leftSideLimit;
        private float _rightSideLimit;

        public float LeftSideLimitOffset;
        public float RightSideLimitOffset;
        public float Speed;
        public float Acceleration;

        protected Vector2 Position { get { return _transform.position; } }
        protected float HorizontalSpeed { get { return _rigidBody2D.velocity.x; } }

        protected override void Awake()
        {
            base.Awake();

            _transform = transform;
            _rigidBody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _leftSideLimit = _transform.position.x - LeftSideLimitOffset;
            _rightSideLimit = _transform.position.x + RightSideLimitOffset;

            float midPoint = _rightSideLimit - _leftSideLimit;
            float startDirection = Mathf.Sign(midPoint - _transform.position.x);

            _rigidBody2D.velocity = new Vector2(Speed * startDirection, 0.0f);
            _transform.localScale = new Vector3(_transform.localScale.x * startDirection, _transform.localScale.y, _transform.localScale.z);
        }

        protected virtual void Update()
        {
            if (_transform.position.x <= _leftSideLimit) { ApplyAcceleration(Acceleration); }
            if (_transform.position.x >= _rightSideLimit) { ApplyAcceleration(-Acceleration); }
        }

        protected virtual void ApplyAcceleration(float acceleration)
        {
            float currentDirection = _rigidBody2D.velocity.x == 0.0f ? 0.0f : Mathf.Sign(_rigidBody2D.velocity.x);
            _rigidBody2D.velocity = new Vector2(Mathf.Clamp(_rigidBody2D.velocity.x + acceleration, -Speed, Speed), _rigidBody2D.velocity.y);

            if ((_rigidBody2D.velocity.x != 0.0f) && (Mathf.Sign(_rigidBody2D.velocity.x) != currentDirection))
            {
                _transform.localScale = new Vector3(
                    Mathf.Abs(_transform.localScale.x) * Mathf.Sign(_rigidBody2D.velocity.x), _transform.localScale.y, _transform.localScale.z);
            }
        }
    }
}