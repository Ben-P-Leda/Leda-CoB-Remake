using UnityEngine;

using Shared.Scripts;

using Gameplay.Shared.Scripts.Player;

namespace Gameplay.Normal.Scripts.Player
{
    public class NormalPlayerController : InputDrivenPlayer
    {
        private NormalPlayerSequencer _sequenceController;

        private Vector2 _gateCenter;
        private bool _enteringGate;
        private GateType _activeGateType;

        protected override void Awake()
        {
            base.Awake();

            _sequenceController = _transform.parent.GetComponent<NormalPlayerSequencer>();
            SequenceController = _sequenceController;
        }

        protected override void Reset()
        {
            base.Reset();

            _activeGateType = GateType.None;
            _gateCenter = Vector2.zero;
            _enteringGate = false;
        }

        protected override void Update()
        {
            base.Update();

            CheckForGateActivation();
            CheckForGateEntry();
        }

        protected override int GetDirection()
        {
            int direction = 0;
            if (_enteringGate)
            {
                direction = GetDirectionFromGateCenter();
            }
            else
            {
                direction = base.GetDirection();
            }

            return direction;
        }

        private int GetDirectionFromGateCenter()
        {
            return (int)Mathf.Sign(_gateCenter.x - _transform.position.x);
        }

        protected override void CheckForJumpStart()
        {
            if (!_enteringGate)
            {
                base.CheckForJumpStart();
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collider)
        {
            base.OnTriggerEnter2D(collider);

            switch (collider.tag)
            {
                case "Exit Gate": _gateCenter = collider.transform.position; _activeGateType = GateType.Exit; break;
                case "Warp Gate": _gateCenter = collider.transform.position; _activeGateType = GateType.Warp; break;
            }
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

        private const float Gate_Entry_Center_Proximity = 0.05f;
    }
}
