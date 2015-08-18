using UnityEngine;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class Type07Movement : MonoBehaviour, ICanBeFrozen
    {
        private Transform _transform;
        private SpriteRenderer _spriteRenderer;
        private GameObject _splashEffect;
        private GameObject _damageComponent;
        private Rigidbody2D _rigidBody2D;
        private Vector2 _velocityBeforeFreeze;
        private State _state;
        private float _stateTimer;
        private Vector3 _startPosition;

        public bool Frozen { get; set; }

        private void Awake()
        {
            _transform = transform;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _splashEffect = _transform.FindChild("Splash Effect").gameObject;
            _damageComponent = _transform.FindChild("Damage Component").gameObject;

            _startPosition = _transform.position;
            StartRegeneration();
        }

        protected virtual void Update()
        {
            if (ShouldSwitchFreezeState) { SwitchFreezeState(); }
            else { HandleStateUpdate(); }

            _stateTimer = Mathf.Max(_stateTimer - Time.deltaTime, 0.0f);
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

        private void HandleStateUpdate()
        {
            if (_state == State.Regenerating) { Regenerate(); }
            if ((_rigidBody2D.velocity.y >= -0.00001f) && (_state == State.Falling)) { CompleteFall(); }
            if ((_state == State.AwaitingRegeneration) && (_stateTimer <= 0.0f)) { StartRegeneration(); }
        }

        private void Regenerate()
        {
            _transform.localScale = new Vector3(1.0f, Mathf.Clamp01(1.0f - (_stateTimer / Regeneration_Time_Base)), 1.0f);
            if (_stateTimer <= 0.0f)
            {
                _rigidBody2D.isKinematic = false;
                if (_rigidBody2D.velocity.y < -0.01f)
                {
                    _transform.localScale = Vector3.one;
                    _state = State.Falling;
                }
            }
        }

        private void CompleteFall()
        {
            _spriteRenderer.enabled = false;
            _damageComponent.SetActive(false);
            _splashEffect.SetActive(true);

            _stateTimer = Regeneration_Delay_Base + (Random.value * Regeneration_Delay_Range);
            _state = State.AwaitingRegeneration;
        }

        private void StartRegeneration()
        {
            _transform.localScale = new Vector3(1.0f, 0.0f, 1.0f);
            _rigidBody2D.velocity = Vector2.zero;
            _rigidBody2D.isKinematic = true;
            _spriteRenderer.enabled = true;
            _damageComponent.SetActive(true);
            _splashEffect.SetActive(false);

            _transform.position = _startPosition;
            _stateTimer = Regeneration_Time_Base;
            _state = State.Regenerating;
        }

        private enum State
        {
            AwaitingRegeneration,
            Regenerating,
            Falling
        }

        private const float Regeneration_Delay_Base = 0.75f;
        private const float Regeneration_Delay_Range = 0.75f;
        private const float Regeneration_Time_Base = 0.5f;
    }
}