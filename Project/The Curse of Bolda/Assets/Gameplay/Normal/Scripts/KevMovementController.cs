using UnityEngine;

namespace Gameplay.Normal.Scripts
{
    public class KevMovementController : MonoBehaviour
    {
        private Transform _transform;
        private Rigidbody2D _rigidBody2D;
        private Animator _animator;
        private bool _facingRight;
        private VerticalMovementState _verticalMovementState;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _facingRight = true;
            _verticalMovementState = VerticalMovementState.OnGround;
        }

        private void Update()
        {
            UpdateVerticalMovement();
            UpdateHorizontalMovement();

            if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
        }

        private void UpdateHorizontalMovement()
        {
            int direction = 0;
            if (Input.GetKey(KeyCode.A)) { direction = -1; }
            if (Input.GetKey(KeyCode.D)) { direction = 1; }

            _rigidBody2D.velocity = new Vector2(direction * Speed, _rigidBody2D.velocity.y);

            if (FacingDirectionHasChanged(direction)) { Flip(); }
            _animator.SetBool("Walking", direction != 0);
        }

        private bool FacingDirectionHasChanged(int movementDirection)
        {
            bool directionHasChanged = false;
            if ((_facingRight) && (movementDirection < 0)) { directionHasChanged = true; }
            if ((!_facingRight) && (movementDirection > 0)) { directionHasChanged = true; }

            return directionHasChanged;
        }

        private void Flip()
        {
            Vector3 currentScale = _transform.localScale;
            currentScale.x *= -1;
            _transform.localScale = currentScale;

            _facingRight = !_facingRight;
        }

        private void UpdateVerticalMovement()
        {
            if ((_rigidBody2D.velocity.y < Fall_Velocity_Threshold) && (_verticalMovementState != VerticalMovementState.Falling)) { StartFalling(); }

            switch (_verticalMovementState)
            {
                case VerticalMovementState.OnGround: CheckForJumpStart(); break;
                case VerticalMovementState.Falling: HandleFalling(); break;
            }
        }

        private void StartFalling()
        {
            _verticalMovementState = VerticalMovementState.Falling;
            _animator.SetBool("Falling", true);
            _animator.SetBool("Jumping", false);
        }

        private void CheckForJumpStart()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                _rigidBody2D.AddForce(new Vector2(0.0f, Jump_Power));
                _verticalMovementState = VerticalMovementState.Rising;
                _animator.SetBool("Jumping", true);
            }
        }

        private void HandleFalling()
        {
            if (_rigidBody2D.velocity.y >= 0.0f)
            {
                _verticalMovementState = VerticalMovementState.OnGround;
                _animator.SetBool("Falling", false);
                _animator.SetBool("Walking", _rigidBody2D.velocity.x != 0.0f);
            }
        }

        private enum VerticalMovementState
        {
            OnGround,
            Rising,
            Falling
        }

        private const float Speed = 2.0f;
        private const float Jump_Power = 300.0f;
        private const float Fall_Velocity_Threshold = -0.6f;
    }
}