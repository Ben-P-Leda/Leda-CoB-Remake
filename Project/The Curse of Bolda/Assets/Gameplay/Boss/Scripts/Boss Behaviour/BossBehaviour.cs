using UnityEngine;

namespace Gameplay.Boss.Scripts.Boss_Behaviour
{
    public class BossBehaviour : MonoBehaviour
    {
        private Transform _transform;
        private Rigidbody2D _rigidBody2D;
        private Animator _animator;
        private BossFrontCollider _forwardCollider;
        private string[] _animationParameters;
        private Transform _playerTransform;
        private float _timedActionTimer;

        private float _minimumPercentileToFire;
        private float _minimumPercentileToStamp;
        private float _minimumPercentileToJump;

        private Vector3 _startingPosition;

        public Behaviour ActiveBehaviour { get; private set; }

        public GameObject Player;
        public float FireChance;
        public float StampChance;
        public float JumpChance;

        private void Awake()
        {
            _transform = transform;
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _forwardCollider = transform.FindChild("Front Collider").GetComponent<BossFrontCollider>();

            _animationParameters = new string[] { "Walking", "Jumping", "Falling", "Landing", "Stamping", "Firing" };

            _startingPosition = _transform.position;

            _playerTransform = Player.transform;

            _minimumPercentileToFire = 100.0f - FireChance;
            _minimumPercentileToStamp = _minimumPercentileToFire - StampChance;
            _minimumPercentileToJump = _minimumPercentileToStamp - JumpChance;

            ActiveBehaviour = Behaviour.WaitingForPlayer;
        }

        public void Activate()
        {
            ActiveBehaviour = Behaviour.WaitingForPlayer;
            SelectNextAction();

            _timedActionTimer = 5.0f;
        }

        private void SelectNextAction()
        {
            if (ActiveBehaviour != Behaviour.Resting)
            {
                StartAction(Behaviour.Resting);
            }
            else
            {
                float actionPercentile = Random.Range(0.0f, 99.0f);
                if (actionPercentile >= _minimumPercentileToFire) { StartAction(Behaviour.Firing); }
                else if (actionPercentile >= _minimumPercentileToStamp) { StartAction(Behaviour.Stamping); }
                //else if (actionPercentile >= _minimumPercentileToJump) { StartAction(Behaviour.Jumping); }
                else { StartAction(Behaviour.Walking); }
            }
        }

        private void StartAction(Behaviour actionToStart)
        {
            FacePlayer();
            ActiveBehaviour = actionToStart;

            _rigidBody2D.velocity = new Vector2(0.0f, _rigidBody2D.velocity.y);
            _timedActionTimer = 0.0f;

            switch (actionToStart)
            {
                case Behaviour.Resting: StartResting(); break;
                case Behaviour.Walking: StartWalking(); break;
                case Behaviour.Stamping: SetAnimationFlags("Stamping"); break;
                case Behaviour.Firing: SetAnimationFlags("Firing"); break;
            }
        }

        private void FacePlayer()
        {
            Vector3 currentScale = _transform.localScale;
            currentScale.x = DirectionToPlayer;
            _transform.localScale = currentScale;
        }

        private float DirectionToPlayer { get { return Mathf.Sign(_playerTransform.position.x - _transform.position.x); } }

        private void StartResting()
        {
            SetAnimationFlags("Resting");
            StartActionTimer(1.0f, 2.0f);
        }

        private void SetAnimationFlags(string flagToActivate)
        {
            for (int i = 0; i < _animationParameters.Length; i++)
            {
                _animator.SetBool(_animationParameters[i], (_animationParameters[i] == flagToActivate));
            }
        }

        private void StartActionTimer(float minimumDuration, float maximumDuration)
        {
            _timedActionTimer = Random.Range(minimumDuration, maximumDuration);
        }


        private void StartWalking()
        {
            SetAnimationFlags("Walking");
            StartActionTimer(3.0f, 5.0f);

            _rigidBody2D.velocity = new Vector2(DirectionToPlayer * Walk_Speed, _rigidBody2D.velocity.y);
        }

        private void Update()
        {
            if (_timedActionTimer > 0.0f) { HandleTimerUpdate(); }
            if ((ActiveBehaviour == Behaviour.Walking) && (_forwardCollider.IsInCollision)) { SelectNextAction(); }
        }

        private void HandleTimerUpdate()
        {
            _timedActionTimer -= Time.deltaTime;
            if (_timedActionTimer <= 0.0f)
            {
                SelectNextAction();
            }
        }

        private void StartRockfall()
        {

        }

        private void StartFireWave()
        {

        }

        private void CompleteAttackAction()
        {
            SelectNextAction();
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

        private float Walk_Speed = 0.8f;
    }
}
