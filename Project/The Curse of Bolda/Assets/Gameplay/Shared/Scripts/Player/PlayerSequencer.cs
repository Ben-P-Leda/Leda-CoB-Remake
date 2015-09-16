using UnityEngine;

using Shared.Scripts;
using Gameplay.Shared.Scripts.Shots;
using Gameplay.Shared.Scripts.Effects;
using Gameplay.Normal.Scripts.Player;

namespace Gameplay.Shared.Scripts.Player
{
    public class PlayerSequencer : MonoBehaviour, IPlayerSequencer
    {
        protected Animator _sequencedAvatarAnimator { get; private set; }
        protected Transform _inputDrivenAvatarTransform { get; private set; }
        protected Transform _sequencedAvatarTransform { get; private set; }
        protected SequencedPlayer _sequencedAvatarController { get; private set; }
        protected TrackingCameraController _cameraController { get; private set; }
        protected KevShotPool _basicShotPool { get; private set; }

        protected GameObject _notEnoughGemsPopup { get; private set; }

        private bool _sequenceRunning;

        public PlayerDeathSequence DeathSequence { private get; set; }

        public SequencedPlayer.PlayerSequenceCompleteCallback SequenceCompletionHandler
        {
            set { _sequencedAvatarController.SequenceCompleteHandler = value; }
        }

        public GameObject Camera;
        public GameObject InputDrivenAvatar;
        public GameObject SequencedAvatar;

        protected virtual void Awake()
        {
            _sequencedAvatarTransform = SequencedAvatar.GetComponent<Transform>();
            _sequencedAvatarAnimator = SequencedAvatar.GetComponent<Animator>();
            _sequencedAvatarController = SequencedAvatar.GetComponent<SequencedPlayer>();
            _cameraController = Camera.GetComponent<TrackingCameraController>();

            _basicShotPool = transform.FindChild("Kev Basic Shots").GetComponent<KevShotPool>();
            _notEnoughGemsPopup = transform.FindChild("Not Enough Gems Sequencer").gameObject;

            _sequenceRunning = false;

            SequencedAvatar.SetActive(false);
        }

        public virtual void StartDeathSequence(PlayerDeathSequence deathSequence, SequencedPlayer.PlayerSequenceCompleteCallback sequenceCompleteCallback)
        {
            if (!_sequenceRunning)
            {
                SwitchToSequencedAvatar();

                _sequencedAvatarController.SequenceCompleteHandler = sequenceCompleteCallback;
                _sequencedAvatarAnimator.SetInteger("DeathSequence", 1);
                _sequencedAvatarTransform.FindChild("Generic Death Particles").gameObject.SetActive(true);
            }
        }

        protected void SwitchToSequencedAvatar()
        {
            _basicShotPool.CanShoot = false;

            InputDrivenAvatar.SetActive(false);

            _sequencedAvatarTransform.position = _inputDrivenAvatarTransform.position;
            SequencedAvatar.SetActive(true);
        }

        public virtual void StartNewLife()
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
            _notEnoughGemsPopup.SetActive(false);

            _basicShotPool.CanShoot = true;

            DeathSequence = PlayerDeathSequence.Generic;

            // TODO: "Let's rock!" sequence
        }

        public const int Player_Physics_Layer_Index = 1024;
    }
}