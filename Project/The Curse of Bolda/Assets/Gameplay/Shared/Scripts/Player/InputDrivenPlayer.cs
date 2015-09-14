using UnityEngine;

using Shared.Scripts;
using Gameplay.Shared.Scripts.Effects;

namespace Gameplay.Shared.Scripts.Player
{
    public class InputDrivenPlayer : MonoBehaviour
    {
        private Transform _transform;
        private Rigidbody2D _rigidBody2D;
        private Animator _animator;
        private PlayerSequencer _sequenceController;

        private GameObject _invincibilityEffect;
        private GameObject _pickaxe;

        private bool _facingRight;
        private VerticalMovementState _verticalMovementState;
        private bool _isMoving;
        private bool _lockMovement;
        private Vector2 _gateCenter;
        private bool _enteringGate;
        private GateType _activeGateType;

        public bool PickaxeHasGripped { private get; set; }

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            _sequenceController = _transform.parent.GetComponent<PlayerSequencer>();
            _invincibilityEffect = _transform.FindChild("Invincibility Effect").gameObject;
            _pickaxe = _transform.FindChild("Kev Body").FindChild("Kev Bicep Front").FindChild("Kev Forearm Front").FindChild("Pickaxe").gameObject;
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
            _isMoving = false;

            _activeGateType = GateType.None;
            _gateCenter = Vector2.zero;
            _enteringGate = false;

            _invincibilityEffect.SetActive(false);
            _animator.SetBool("Jetpack", false);
            _animator.SetBool("Extinguisher", false);
            _animator.SetBool("Walking", false);
            _animator.SetBool("Pickaxe", false);
            _rigidBody2D.gravityScale = 1.0f;
            _lockMovement = false;

            PickaxeHasGripped = false;

            CurrentGame.DeactivateTool();
        }

        private void Update()
        {
            _isMoving = false;

            UpdateVerticalMovement();
            UpdateHorizontalMovement();

            CheckForToolActivation();
            CheckForToolDeactivation();

            CheckForGateActivation();
            CheckForGateEntry();

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

            if ((PickaxeHasGripped) && (_verticalMovementState == VerticalMovementState.Falling))
            {
                _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0.0f);
                _rigidBody2D.gravityScale = 0.0f;
                _verticalMovementState = VerticalMovementState.OnGround;
                _animator.SetBool("Pickaxe", true);
            }
            else if ((_rigidBody2D.gravityScale != 1.0f) && ((!PickaxeHasGripped) || (_verticalMovementState == VerticalMovementState.Rising)))
            {
                _animator.SetBool("Pickaxe", false);
                _rigidBody2D.gravityScale = 1.0f;
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
                if (CurrentGame.GameData.ActiveTool == ToolType.SuperJump)
                {
                    _rigidBody2D.AddForce(new Vector2(0.0f, Super_Jump_Power));
                    CurrentGame.UseSuperJump();
                }
                else
                {
                    _rigidBody2D.AddForce(new Vector2(0.0f, Jump_Power));
                }
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
            return (int)Mathf.Sign(_gateCenter.x - _transform.position.x);
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
            if ((Input.GetKeyDown(KeyCode.F1)) && (CurrentGame.HasTool(ToolType.Invincibility))) { ActivateInvincibility(); }
            if ((Input.GetKeyDown(KeyCode.F2)) && (CurrentGame.HasTool(ToolType.Jetpack))) { ActivateJetpack(); }
            if ((Input.GetKeyDown(KeyCode.F3)) && (CurrentGame.HasTool(ToolType.SuperJump))) { ActivateSuperJump(); }
            if ((Input.GetKeyDown(KeyCode.F5)) && (CurrentGame.HasTool(ToolType.Pickaxe))) { ActivatePickaxe(); }
            if ((Input.GetKeyDown(KeyCode.F6)) && (CurrentGame.HasTool(ToolType.FireExtinguisher)) && (!_isMoving)) { ActivateFireExtinguisher(); }

            if ((CurrentGame.GameData.ActiveTool == ToolType.Invincibility) && (!_invincibilityEffect.activeInHierarchy))
            {
                _invincibilityEffect.SetActive(true);
            }
        }

        private void ActivateInvincibility()
        {
            if ((!CurrentGame.ToolIsActive) || (CurrentGame.GameData.ActiveTool == ToolType.SuperJump))
            {
                _invincibilityEffect.SetActive(true);

                CurrentGame.ActivateTool(ToolType.Invincibility);
            }
        }

        private void ActivateJetpack()
        {
            if ((!CurrentGame.ToolIsActive) || (CurrentGame.GameData.ActiveTool == ToolType.SuperJump))
            {
                _animator.SetBool("Jumping", false);
                _animator.SetBool("Walking", false);
                _animator.SetBool("Jetpack", true);

                _rigidBody2D.gravityScale = 0.0f;
                _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, 0.0f);
                _rigidBody2D.AddForce(new Vector2(0.0f, 100.0f));

                CurrentGame.ActivateTool(ToolType.Jetpack);
            }
        }

        private void ActivateSuperJump()
        {
            if (!CurrentGame.ToolIsActive)
            {
                CurrentGame.ActivateTool(ToolType.SuperJump);
            }
            else if (CurrentGame.GameData.ActiveTool == ToolType.SuperJump)
            {
                CurrentGame.GameData.ActiveTool = ToolType.None;
            }
        }

        private void ActivatePickaxe()
        {
            if ((!CurrentGame.ToolIsActive) || (CurrentGame.GameData.ActiveTool == ToolType.SuperJump))
            {
                _pickaxe.SetActive(true);

                CurrentGame.ActivateTool(ToolType.Pickaxe);
            }
        }

        private void ActivateFireExtinguisher()
        {
            if ((!CurrentGame.ToolIsActive) || (CurrentGame.GameData.ActiveTool == ToolType.SuperJump))
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
                ResetToolEffects();
            }
        }

        private void ResetToolEffects()
        {
            _invincibilityEffect.SetActive(false);
            _pickaxe.SetActive(false);
            _animator.SetBool("Jetpack", false);
            _animator.SetBool("Extinguisher", false);
            _animator.SetBool("Walking", false);
            _animator.SetBool("Pickaxe", false);
            _rigidBody2D.gravityScale = 1.0f;
            _lockMovement = false;

            PickaxeHasGripped = false;

            CurrentGame.DeactivateTool();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            switch (collider.tag)
            {
                case "Exit Gate": _gateCenter = collider.transform.position; _activeGateType = GateType.Exit; break;
                case "Warp Gate": _gateCenter = collider.transform.position; _activeGateType = GateType.Warp; break;
                case "Water Surface": WaterSplashPool.ActivateWaterSplash(_transform.position + new Vector3(0, -0.15f, 0)); break;
                case "Water Pool": TriggerDeathSequence(PlayerDeathSequence.Drowning); break;
            }
        }

        private void TriggerDeathSequence(PlayerDeathSequence sequenceToRun)
        {
            _lockMovement = true;
            _sequenceController.DeathSequence = sequenceToRun;
            CurrentGame.GameData.Energy = 0;
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if ((collider.tag == "Exit Gate") || (collider.tag == "Warp Gate"))
            {
                _gateCenter = Vector2.zero;
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
                if (_gateCenter.x <= 0.0f) { return false; }
                if (CurrentGame.ToolIsActive) { return false; }

                return true;
            }
        }

        private void CheckForGateEntry()
        {
            if ((_enteringGate) && (Mathf.Abs(_gateCenter.x - _transform.position.x) < Gate_Entry_Center_Proximity))
            {
                _enteringGate = false;
                _sequenceController.EnterGate(_activeGateType, _gateCenter);
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
        private const float Super_Jump_Power = 2900.0f;
        private const float Normal_Jump_Max_Vertical_Velocity = 5.6f;
        private const float Fall_Velocity_Threshold = -0.6f;
        private const float Touchdown_Velocity_Threshold = -0.001f;
        private const float Gate_Entry_Center_Proximity = 0.05f;
    }
}