using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Normal.Scripts.Player_Control
{
    public class KevActionController : MonoBehaviour
    {
        private Transform _transform;
        private Rigidbody2D _rigidBody2D;
        private Animator _animator;

        private bool _facingRight;
        private VerticalMovementState _verticalMovementState;
        private bool _isMoving;
        private bool _lockMovement;

        private ToolType? _activeTool;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            Reset();
        }

        private void OnEnable()
        {
            Reset();
        }

        private void Reset()
        {
            _facingRight = (_transform.localScale.x > 0.0f);
            _verticalMovementState = VerticalMovementState.OnGround;

            _lockMovement = false;
            _isMoving = false;

            _activeTool = null;
        }

        private void Update()
        {
            _isMoving = false;

            UpdateVerticalMovement();
            UpdateHorizontalMovement();

            CheckForToolActivation();
            CheckForToolDeactivation();

            if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
        }

        private void UpdateHorizontalMovement()
        {
            if (!_lockMovement)
            {
                int direction = 0;
                if (Input.GetKey(KeyCode.A)) { direction = -1; }
                if (Input.GetKey(KeyCode.D)) { direction = 1; }

                _rigidBody2D.velocity = new Vector2(direction * Speed, _rigidBody2D.velocity.y);

                if (FacingDirectionHasChanged(direction)) { Flip(); }
                _animator.SetBool("Walking", direction != 0);

                if (direction != 0) { _isMoving = true; }
            }
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

            if (_verticalMovementState != VerticalMovementState.OnGround) { _isMoving = true; }
        }

        private void StartFalling()
        {
            _verticalMovementState = VerticalMovementState.Falling;
            _animator.SetBool("Falling", true);
            _animator.SetBool("Jumping", false);
        }

        private void CheckForJumpStart()
        {
            if ((!_lockMovement) && (Input.GetKeyDown(KeyCode.W)))
            {
                _rigidBody2D.AddForce(new Vector2(0.0f, Jump_Power));
                _verticalMovementState = VerticalMovementState.Rising;
                _animator.SetBool("Jumping", true);
            }
        }

        private void HandleFalling()
        {
            if (_rigidBody2D.velocity.y >= Touchdown_Velocity_Threshold)
            {
                _verticalMovementState = VerticalMovementState.OnGround;
                _animator.SetBool("Falling", false);
                _animator.SetBool("Walking", _rigidBody2D.velocity.x != 0.0f);
            }
        }

        private void CheckForToolActivation()
        {
            if ((Input.GetKeyDown(KeyCode.F6)) && (CurrentGame.HasTool(ToolType.FireExtinguisher)) && (!_isMoving)) { ActivateFireExtinguisher(); }
        }

        private void ActivateFireExtinguisher()
        {
            _lockMovement = true;
            _animator.SetBool("Extinguisher", true);

            CurrentGame.ActivateTool(ToolType.FireExtinguisher);
            _activeTool = ToolType.FireExtinguisher;
        }

        private void CheckForToolDeactivation()
        {
            if ((_activeTool != null) && (!CurrentGame.ToolIsActive))
            {
                _animator.SetBool("Extinguisher", false);
                _lockMovement = false;
                _activeTool = null;
            }
        }

        private enum VerticalMovementState
        {
            OnGround,
            Rising,
            Falling
        }

        private const float Speed = 2.0f;
        private const float Jump_Power = 290.0f;
        private const float Fall_Velocity_Threshold = -0.6f;
        private const float Touchdown_Velocity_Threshold = -0.001f;
    }
}