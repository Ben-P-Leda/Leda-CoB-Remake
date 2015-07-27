using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Player
{
    public class PlayerSequencer : MonoBehaviour
    {
        private Animator _animator;
        private Transform _inputDrivenAvatarTransform;
        private Transform _sequencedAvatarTransform;
        private FadeTransitioner _fadeTransitioner;

        private SequenceState _state;
        private float _sequenceDurationRemaining;

        public GameObject InputDrivenAvatar;
        public GameObject SequencedAvatar;
        public GameObject CrossFader;

        private void Awake()
        {
            _inputDrivenAvatarTransform = InputDrivenAvatar.GetComponent<Transform>();
            _sequencedAvatarTransform = SequencedAvatar.GetComponent<Transform>();
            _animator = SequencedAvatar.GetComponent<Animator>();
            _fadeTransitioner = CrossFader.GetComponent<FadeTransitioner>();

            _state = SequenceState.Ready;

            SequencedAvatar.SetActive(false);
        }

        private void Update()
        {
            if (_state == SequenceState.Ready)
            {
                if (CurrentGame.GameData.Energy <= 0.0f) { StartDeathSequence(); }
            }
            else if (_state == SequenceState.Running)
            {
                _sequenceDurationRemaining -= Time.deltaTime;
                if (_sequenceDurationRemaining <= 0) { HandleSequenceCompletion(); }
            }
        }

        private void StartDeathSequence()
        {
            InputDrivenAvatar.SetActive(false);

            _sequencedAvatarTransform.position = _inputDrivenAvatarTransform.position;
            SequencedAvatar.SetActive(true);

            _animator.SetInteger("DeathSequence", 1);
            _sequencedAvatarTransform.FindChild("Generic Death Particles").gameObject.SetActive(true);

            _state = SequenceState.Running;
            _sequenceDurationRemaining = 1.0f;
        }

        private void HandleSequenceCompletion()
        {
            _state = SequenceState.Complete;
            _fadeTransitioner.FadeOut();
        }

        private enum SequenceState
        {
            Ready,
            Running,
            Complete
        }
    }
}