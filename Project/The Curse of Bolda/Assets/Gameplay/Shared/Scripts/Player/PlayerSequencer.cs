using UnityEngine;

using Shared.Scripts;
using Gameplay.Shared.Scripts.Shots;
using Gameplay.Normal.Scripts.Player_Control;

namespace Gameplay.Shared.Scripts.Player
{
    public class PlayerSequencer : MonoBehaviour
    {
        private Animator _sequencedAvatarAnimator;
        private Transform _inputDrivenAvatarTransform;
        private Transform _sequencedAvatarTransform;
        private SequencedPlayer _sequencedAvatarController;
        private WarpTracker _warpTracker;
        private TrackingCameraController _cameraController;
        private KevShotPool _basicShotPool;

        private bool _sequenceRunning;
        private GateType _gateBeingEntered;
        private Vector2 _gateCenter;

        public PlayerDeathSequence DeathSequence { private get; set; }

        public SequencedPlayer.PlayerSequenceCompleteCallback SequenceCompletionHandler
        {
            set { _sequencedAvatarController.SequenceCompleteHandler = value; }
        }

        public GameObject Camera;
        public GameObject InputDrivenAvatar;
        public GameObject SequencedAvatar;

        private void Awake()
        {
            _sequencedAvatarTransform = SequencedAvatar.GetComponent<Transform>();
            _sequencedAvatarAnimator = SequencedAvatar.GetComponent<Animator>();
            _sequencedAvatarController = SequencedAvatar.GetComponent<SequencedPlayer>();
            _cameraController = Camera.GetComponent<TrackingCameraController>();

            _basicShotPool = transform.FindChild("Kev Basic Shots").GetComponent<KevShotPool>();
            _warpTracker = transform.Find("Warp Gate Tracker").GetComponent<WarpTracker>();
            _warpTracker.WarpCompletionCallback = CompleteWarp;

            _sequenceRunning = false;

            SequencedAvatar.SetActive(false);
        }

        public void StartDeathSequence(PlayerDeathSequence deathSequence, SequencedPlayer.PlayerSequenceCompleteCallback sequenceCompleteCallback)
        {
            if (!_sequenceRunning)
            {
                SwitchToSequencedAvatar();

                _sequencedAvatarController.SequenceCompleteHandler = sequenceCompleteCallback;
                _sequencedAvatarAnimator.SetInteger("DeathSequence", 1);
                _sequencedAvatarTransform.FindChild("Generic Death Particles").gameObject.SetActive(true);
            }
        }

        private void SwitchToSequencedAvatar()
        {
            _basicShotPool.CanShoot = false;

            InputDrivenAvatar.SetActive(false);

            _sequencedAvatarTransform.position = _inputDrivenAvatarTransform.position;
            SequencedAvatar.SetActive(true);
        }

        public void StartNewLife()
        {
            if ((SequencedAvatar.activeInHierarchy) && (_sequencedAvatarAnimator != null))
            {
                _sequencedAvatarAnimator.SetInteger("DeathSequence", 0);
                _sequencedAvatarTransform.FindChild("Generic Death Particles").gameObject.SetActive(false);
            }

            _inputDrivenAvatarTransform = InputDrivenAvatar.GetComponent<Transform>();
            _inputDrivenAvatarTransform.position = CurrentGame.GameData.RestartPoint;
            _inputDrivenAvatarTransform.localScale = CurrentGame.GameData.RestartScale;
            _cameraController.TransformToTrack = _inputDrivenAvatarTransform;

            SequencedAvatar.SetActive(false);
            InputDrivenAvatar.SetActive(true);
            _warpTracker.Reset();

            _basicShotPool.CanShoot = true;

            DeathSequence = PlayerDeathSequence.Generic;

            // TODO: "Let's rock!" sequence
        }

        public void EnterGate(GateType gateBeingEntered, Vector2 gateCenter)
        {
            _gateBeingEntered = gateBeingEntered;
            _gateCenter = gateCenter;

            CurrentGame.GameData.TimerIsFrozen = true;
            SwitchToSequencedAvatar();
            _sequencedAvatarController.SequenceCompleteHandler = CompleteGateEntrySequence;
            _sequencedAvatarAnimator.SetInteger("DeathSequence", 0);
            _sequencedAvatarAnimator.SetBool("EnteringGate", true);
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
            SequencedAvatar.SetActive(false);
            _warpTracker.Activate(_gateCenter);
            _cameraController.TransformToTrack = _warpTracker.transform;
        }

        public void CompleteWarp()
        {
            // TODO: Initiate gate exit sequence

            InputDrivenAvatar.SetActive(true);
            _inputDrivenAvatarTransform.position = _warpTracker.transform.position;
            _cameraController.TransformToTrack = _inputDrivenAvatarTransform;
            _warpTracker.Reset();
        }

        public const int Player_Physics_Layer_Index = 1024;
    }
}