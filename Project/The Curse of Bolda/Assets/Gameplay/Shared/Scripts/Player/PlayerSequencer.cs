using UnityEngine;

using Shared.Scripts;
using Gameplay.Shared.Scripts.Shots;

namespace Gameplay.Shared.Scripts.Player
{
    public class PlayerSequencer : MonoBehaviour
    {
        public delegate void PlayerSequenceCompleteCallback();

        private Animator _animator;
        private Transform _inputDrivenAvatarTransform;
        private Transform _sequencedAvatarTransform;
        private KevShotPool _basicShotPool;

        private SequenceState _state;
        private float _sequenceDurationRemaining;

        public GameObject InputDrivenAvatar;
        public GameObject SequencedAvatar;

        public PlayerSequenceCompleteCallback SequenceCompleteHandler { private get; set; }

        private void Awake()
        {
            _sequencedAvatarTransform = SequencedAvatar.GetComponent<Transform>();
            _animator = SequencedAvatar.GetComponent<Animator>();

            _basicShotPool = transform.FindChild("Kev Basic Shots").GetComponent<KevShotPool>();

            _state = SequenceState.Ready;

            SequencedAvatar.SetActive(false);
        }

        private void Update()
        {
            if (_state == SequenceState.Running)
            {
                _sequenceDurationRemaining -= Time.deltaTime;
                if (_sequenceDurationRemaining <= 0) { HandleSequenceCompletion(); }
            }
        }

        public void StartDeathSequence(PlayerDeathSequence deathSequence)
        {
            if (_state == SequenceState.Ready)
            {
                _basicShotPool.CanShoot = false;

                InputDrivenAvatar.SetActive(false);

                _sequencedAvatarTransform.position = _inputDrivenAvatarTransform.position;
                SequencedAvatar.SetActive(true);

                _animator.SetInteger("DeathSequence", 1);
                _sequencedAvatarTransform.FindChild("Generic Death Particles").gameObject.SetActive(true);

                _state = SequenceState.Running;
                _sequenceDurationRemaining = 1.0f;
            }
        }

        private void HandleSequenceCompletion()
        {
            _state = SequenceState.Complete;
            if (SequenceCompleteHandler != null) { SequenceCompleteHandler(); }
            SequenceCompleteHandler = null;
        }

        public void StartNewLife()
        {
            _state = SequenceState.Ready;

            _inputDrivenAvatarTransform = InputDrivenAvatar.GetComponent<Transform>();
            _inputDrivenAvatarTransform.position = CurrentGame.GameData.RestartPoint;
            _inputDrivenAvatarTransform.localScale = CurrentGame.GameData.RestartScale;

            SequencedAvatar.SetActive(false);
            InputDrivenAvatar.SetActive(true);

            _basicShotPool.CanShoot = true;

            // TODO: "Let's rock!" sequence
        }

        private enum SequenceState
        {
            Ready,
            Running,
            Complete
        }
    }
}