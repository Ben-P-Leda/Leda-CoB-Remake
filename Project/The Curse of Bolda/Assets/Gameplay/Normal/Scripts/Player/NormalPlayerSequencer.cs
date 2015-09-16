using UnityEngine;

using Shared.Scripts;

using Gameplay.Shared.Scripts.Player;

namespace Gameplay.Normal.Scripts.Player
{
    public class NormalPlayerSequencer : PlayerSequencer
    {
        private WarpTracker _warpTracker;
        private GateType _gateBeingEntered;
        private Vector2 _gateCenter;

        protected override void Awake()
        {
            base.Awake();

            _warpTracker = transform.Find("Warp Gate Tracker").GetComponent<WarpTracker>();
            _warpTracker.WarpCompletionCallback = CompleteWarp;
        }

        public override void StartNewLife()
        {
            base.StartNewLife();

            _warpTracker.Reset();
        }

        public void EnterGate(GateType gateBeingEntered, Vector2 gateCenter)
        {
            if ((gateBeingEntered == GateType.Exit) && (CurrentGame.GameData.GemsCollected < CurrentGame.GameData.GemsRequired))
            {
                if (!_notEnoughGemsPopup.activeInHierarchy)
                {
                    _notEnoughGemsPopup.SetActive(true);
                }
            }
            else
            {
                _gateBeingEntered = gateBeingEntered;
                _gateCenter = gateCenter;

                CurrentGame.GameData.TimerIsFrozen = true;
                SwitchToSequencedAvatar();
                _sequencedAvatarController.SequenceCompleteHandler = CompleteGateEntrySequence;
                _sequencedAvatarAnimator.SetInteger("DeathSequence", 0);
                _sequencedAvatarAnimator.SetBool("ExitingGate", false);
                _sequencedAvatarAnimator.SetBool("EnteringGate", true);
            }
        }

        public void CompleteGateEntrySequence()
        {
            switch (_gateBeingEntered)
            {
                case GateType.Exit: CurrentGame.GameData.GameplayState = GameplayState.LevelComplete; break;
                case GateType.Warp: InitiateWarp(); break;
            }
        }

        public void InitiateWarp()
        {
            _warpTracker.Activate(_gateCenter);
            _cameraController.TransformToTrack = _warpTracker.transform;
        }

        public void CompleteWarp()
        {
            _sequencedAvatarTransform.position = new Vector3(
                _warpTracker.transform.position.x,
                _warpTracker.transform.position.y,
                _sequencedAvatarTransform.position.z);
            _warpTracker.Reset();

            _sequencedAvatarController.SequenceCompleteHandler = CompleteGateExitSequence;
            _sequencedAvatarAnimator.SetBool("EnteringGate", false);
            _sequencedAvatarAnimator.SetBool("ExitingGate", true);

            _cameraController.TransformToTrack = _sequencedAvatarTransform;
        }

        private void CompleteGateExitSequence()
        {
            _basicShotPool.CanShoot = true;

            _sequencedAvatarAnimator.SetBool("EnteringGate", false);
            _sequencedAvatarAnimator.SetBool("ExitingGate", true);
            SequencedAvatar.SetActive(false);

            _inputDrivenAvatarTransform.position = _sequencedAvatarTransform.position;
            _cameraController.TransformToTrack = _inputDrivenAvatarTransform;
            InputDrivenAvatar.SetActive(true);

            CurrentGame.ActivateInvincibilityFollowingWarp();
        }

    }
}
