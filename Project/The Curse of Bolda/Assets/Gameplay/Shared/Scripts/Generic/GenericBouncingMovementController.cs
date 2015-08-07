using UnityEngine;

namespace Gameplay.Shared.Scripts.Generic
{
    public class GenericBouncingMovementController : MonoBehaviour
    {
        private Transform _transform;
        private Rigidbody2D _rigidBody2D;
        private float _bouncePlainY;
        private float _bounceStartDelay;

        public bool DelayBounceStart;

        protected Vector2 Velocity { get { return _rigidBody2D.velocity; } set { _rigidBody2D.velocity = value; } }

        protected virtual void Awake()
        {
            _transform = transform;
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _bouncePlainY = transform.position.y;

            _bounceStartDelay = DelayBounceStart ? Random.Range(0, Maximum_Bounce_Delay_Milliseconds) / 1000.0f : 0.0f;
        }

        protected virtual void Update()
        {
            if (_bounceStartDelay > 0) 
            { 
                _bounceStartDelay -= Time.deltaTime; 
            }
            else
            {
                if (_transform.position.y < _bouncePlainY) { _transform.position = new Vector3(_transform.position.x, _bouncePlainY, _transform.position.z); }
                if (_transform.position.y <= _bouncePlainY + Bounce_Plain_Tolerance) { HandleGroundImpact(); }
            }
        }

        protected virtual void HandleGroundImpact()
        {
            _rigidBody2D.velocity = new Vector2(0.0f, 5.0f);
        }

        private const float Bounce_Plain_Tolerance = 0.05f;
        private const int Maximum_Bounce_Delay_Milliseconds = 3000;
    }
}
