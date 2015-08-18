using UnityEngine;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class Type10Movement : MonoBehaviour, ICanBeFrozen
    {
        private Transform _transform;
        private Rigidbody2D _rigidBody2D;
        private Vector2 _velocityBeforeFreeze;
        private Transform _playerTransform;

        public bool Frozen { get; set; }

        public GameObject Player;

        private void Awake()
        {
            _transform = transform;
            _rigidBody2D = GetComponent<Rigidbody2D>();

            _playerTransform = Player.transform;
        }

        protected virtual void Update()
        {
            if (ShouldSwitchFreezeState) { SwitchFreezeState(); }
            if (ShouldMoveTowardsPlayer) { ApplyAcceleration(); }
        }

        public virtual void SwitchFreezeState()
        {
            if (Frozen)
            {
                _velocityBeforeFreeze = _rigidBody2D.velocity;
                _rigidBody2D.Sleep();
            }
            else
            {
                _rigidBody2D.WakeUp();
                _rigidBody2D.velocity = _velocityBeforeFreeze;
            }
        }

        private bool ShouldSwitchFreezeState
        {
            get
            {
                if ((_rigidBody2D.IsSleeping()) && (!Frozen)) { return true; }
                if ((!_rigidBody2D.IsSleeping()) && (Frozen)) { return true; }
                return false;
            }
        }

        private bool ShouldMoveTowardsPlayer
        {
            get
            {
                if (Frozen) { return false; }
                if (!Player.activeInHierarchy) { return false; }
                if (Vector2.Distance(_transform.position, _playerTransform.position) > Activation_Proximity) { return false; }

                return true;
            }
        }

        private void ApplyAcceleration()
        {
            Vector2 translationToPlayer = new Vector2(
                _playerTransform.position.x - _transform.position.x, _playerTransform.position.y - _transform.position.y);

            Vector2 velocityStep = translationToPlayer.normalized * Acceleration;

            Vector2 velocity = new Vector2(
                Mathf.Clamp(_rigidBody2D.velocity.x + velocityStep.x, -Maximum_Speed, Maximum_Speed),
                Mathf.Clamp(_rigidBody2D.velocity.y + velocityStep.y, -Maximum_Speed, Maximum_Speed));

            _rigidBody2D.velocity = velocity;
        }

        private const float Activation_Proximity = 8.0f;
        private const float Acceleration = 0.1f;
        private const float Maximum_Speed = 3.5f;
    }
}