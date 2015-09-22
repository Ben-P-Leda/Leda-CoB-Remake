using UnityEngine;

namespace Gameplay.Boss.Scripts.Boss_Behaviour
{
    public class BossBehaviour : MonoBehaviour
    {
        private Transform _transform;
        private Rigidbody2D _rigidBody2D;
        private Animator _animator;
        private Transform _playerTransform;

        private Vector3 _startingPosition;

        public Behaviour ActiveBehaviour { get; private set; }

        public GameObject Player;

        private void Awake()
        {
            _transform = transform;
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            _startingPosition = _transform.position;

            _playerTransform = Player.transform;

            ActiveBehaviour = Behaviour.WaitingForPlayer;
        }

        public void Activate()
        {
            ActiveBehaviour = Behaviour.Resting;
            FacePlayer();
        }

        private void FacePlayer()
        {
            Vector3 currentScale = _transform.localScale;
            currentScale.x *= Mathf.Sign(_playerTransform.position.x - _transform.position.x);
            _transform.localScale = currentScale;
        }

        public enum Behaviour
        {
            WaitingForPlayer,
            Resting,
            Walking,
            Jumping,
            Stamping,
            Firing
        }
    }
}
