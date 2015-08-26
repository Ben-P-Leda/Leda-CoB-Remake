using UnityEngine;

using Shared.Scripts;
using Gameplay.Shared.Scripts.Player;

namespace Gameplay.Normal.Scripts.Player_Control
{
    public class KevActionController : MonoBehaviour
    {
        private Transform _transform;
        private Rigidbody2D _rigidBody2D;
        private Animator _animator;
        private PlayerSequencer _sequenceController;

        private bool _facingRight;
        private VerticalMovementState _verticalMovementState;
        private bool _isMoving;
        private bool _lockMovement;
        private float _gateCenterX;
        private bool _enteringGate;
        private GateType _activeGateType;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            _sequenceController = _transform.parent.GetComponent<PlayerSequencer>();
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

            _activeGateType = GateType.None;
            _gateCenterX = 0.0f;
            _enteringGate = false;
        }

        private void Update()
        {
            _isMoving = false;

            UpdateVerticalMovement();
            UpdateHorizontalMovement();

            CheckForToolActivation();
            CheckForToolDeactivation();

            CheckForGateActivation();
            CheckForGateEntry();;

            if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
        }

        private void UpdateVerticalMovement()
        {
            if (CurrentGame.GameData.ActiveTool == ToolType.Jetpack) { HandleJetpackVerticalMovement(); }
            else { HandleNormalVerticalMovement(); }
        }

        private void HandleJetpackVerticalMovement()
        {
            float verticalSpeed = 0.0f;
            if (Input.GetKey(KeyCode.W)) { verticalSpeed = Speed; }
            if (Input.GetKey(KeyCode.S)) { verticalSpeed = -Speed; }

            _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, verticalSpeed);
        }

        private void HandleNormalVerticalMovement()
        {
            if ((_rigidBody2D.velocity.y < Fall_Velocity_Threshold) && (_verticalMovementState != VerticalMovementState.Falling)) { StartFalling(); }
            if (_rigidBody2D.velocity.y > Normal_Jump_Max_Vertical_Velocity)
            {
                _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, Normal_Jump_Max_Vertical_Velocity);
            }

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
            if ((!_lockMovement) && (!_enteringGate) && (Input.GetKeyDown(KeyCode.W)))
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

        private void UpdateHorizontalMovement()
        {
            if (!_lockMovement)
            {
                int direction = 0;
                if (_enteringGate) { direction = GetDirectionFromGateCenter(); }
                else { direction = GetDirectionFromInput(); }

                _rigidBody2D.velocity = new Vector2(direction * Speed, _rigidBody2D.velocity.y);

                if (FacingDirectionHasChanged(direction)) { Flip(); }
                _animator.SetBool("Walking", direction != 0);

                if (direction != 0) { _isMoving = true; }
            }
        }

        private int GetDirectionFromGateCenter()
        {
            return (int)Mathf.Sign(_gateCenterX - _transform.position.x);
        }

        private int GetDirectionFromInput()
        {
            if (Input.GetKey(KeyCode.A)) { return -1; }
            if (Input.GetKey(KeyCode.D)) { return 1; }

            return 0;
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

        private void CheckForToolActivation()
        {
            if ((Input.GetKeyDown(KeyCode.F2)) && (CurrentGame.HasTool(ToolType.Jetpack))) { ActivateJetpack(); }
            if ((Input.GetKeyDown(KeyCode.F6)) && (CurrentGame.HasTool(ToolType.FireExtinguisher)) && (!_isMoving)) { ActivateFireExtinguisher(); }
        }

        private void ActivateJetpack()
        {
            if (!CurrentGame.ToolIsActive)
            {
                _animator.SetBool("Jumping", false);
                _animator.SetBool("Jetpack", true);

                _rigidBody2D.gravityScale = 0.0f;
                _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0.0f);

                CurrentGame.ActivateTool(ToolType.Jetpack);
            }
        }

        private void ActivateFireExtinguisher()
        {
            if (!CurrentGame.ToolIsActive)
            {
                _lockMovement = true;
                _animator.SetBool("Extinguisher", true);

                CurrentGame.ActivateTool(ToolType.FireExtinguisher);
            }
        }

        private void CheckForToolDeactivation()
        {
            if (CurrentGame.ToolReadyForDeactivation)
            {
                _animator.SetBool("Jetpack", false);
                _animator.SetBool("Extinguisher", false);
                _animator.SetBool("Walking", false);
                _rigidBody2D.gravityScale = 1.0f;
                _lockMovement = false;

                CurrentGame.DeactivateTool();
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Exit Gate")
            {
                _gateCenterX = collider.transform.position.x;
                _activeGateType = GateType.Exit;
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.tag == "Exit Gate")
            {
                _gateCenterX = 0.0f;
                _activeGateType = GateType.None;
            }
        }

        private void CheckForGateActivation()
        {
            if ((GateEntryPermitted) && (Input.GetKeyDown(KeyCode.S))) { _enteringGate = true; }
        }

        private bool GateEntryPermitted
        {
            get
            {
                if (_enteringGate) { return false; }
                if (_verticalMovementState != VerticalMovementState.OnGround) { return false; }
                if (_gateCenterX <= 0.0f) { return false; }
                if (CurrentGame.ToolIsActive) { return false; }

                return true;
            }
        }

        private void CheckForGateEntry()
        {
            if ((_enteringGate) && (Mathf.Abs(_gateCenterX - _transform.position.x) <Gate_Entry_Center_Proximity))
            {
                _enteringGate = false;
                _sequenceController.EnterGate(_activeGateType);
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
        private const float Normal_Jump_Max_Vertical_Velocity = 5.6f;
        private const float Fall_Velocity_Threshold = -0.6f;
        private const float Touchdown_Velocity_Threshold = -0.001f;
        private const float Gate_Entry_Center_Proximity = 0.05f;
    }
}