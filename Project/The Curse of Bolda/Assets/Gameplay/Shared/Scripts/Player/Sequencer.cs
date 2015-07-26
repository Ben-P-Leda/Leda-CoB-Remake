using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Player
{
    public class Sequencer : MonoBehaviour
    {
        private Animator _animator;
        private Transform _inputDrivenAvatarTransform;
        private Transform _sequencedAvatarTransform;

        public GameObject InputDrivenAvatar;
        public GameObject SequencedAvatar;

        private bool SequenceIsRunning { get { return SequencedAvatar.activeInHierarchy; } }

        private void Awake()
        {
            _inputDrivenAvatarTransform = InputDrivenAvatar.GetComponent<Transform>();
            _sequencedAvatarTransform = SequencedAvatar.GetComponent<Transform>();
            _animator = SequencedAvatar.GetComponent<Animator>();

            SequencedAvatar.SetActive(false);
        }

        private void Update()
        {
            if (!SequenceIsRunning)
            {
                if (CurrentGame.GameData.Energy <= 0.0f) { StartDeathSequence(); }
            }
        }

        private void StartDeathSequence()
        {
            InputDrivenAvatar.SetActive(false);

            _sequencedAvatarTransform.position = _inputDrivenAvatarTransform.position;
            SequencedAvatar.SetActive(true);

            _animator.SetInteger("DeathSequence", 1);
            _sequencedAvatarTransform.FindChild("Generic Death Particles").gameObject.SetActive(true);
        }
    }
}