using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Player
{
    public class PlayerSequencer : MonoBehaviour
    {
        public delegate void PlayerSequenceCompleteCallback();

        private Animator _animator;
        private Transform _inputDrivenAvatarTransform;
        private Transform _sequencedAvatarTransform;

        private SequenceState _state;
        private float _sequenceDurationRemaining;

        public GameObject InputDrivenAvatar;
        public GameObject SequencedAvatar;

        public PlayerSequenceCompleteCallback SequenceCompleteHandler { private get; set; }

        private void Awake()
        {
            _sequencedAvatarTransform = SequencedAvatar.GetComponent<Transform>();
            _animator = SequencedAvatar.GetComponent<Animator>();

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
            SequencedAvatar.SetActive(false);
            InputDrivenAvatar.SetActive(true);

            _inputDrivenAvatarTransform = InputDrivenAvatar.GetComponent<Transform>();
            _inputDrivenAvatarTransform.position = new Vector3(
                CurrentGame.GameData.RestartPoint.x,
                CurrentGame.GameData.RestartPoint.y,
                _inputDrivenAvatarTransform.position.z);

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