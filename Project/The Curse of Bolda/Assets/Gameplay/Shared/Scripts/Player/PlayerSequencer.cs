using UnityEngine;

using Shared.Scripts;
using Gameplay.Shared.Scripts.Shots;

namespace Gameplay.Shared.Scripts.Player
{
    public class PlayerSequencer : MonoBehaviour
    {
        private Animator _sequencedAvatarAnimator;
        private Transform _inputDrivenAvatarTransform;
        private Transform _sequencedAvatarTransform;
        private SequencedPlayer _sequencedAvatarController;
        private KevShotPool _basicShotPool;

        private bool _sequenceRunning;
        private GateType _gateBeingEntered;

        public SequencedPlayer.PlayerSequenceCompleteCallback SequenceCompletionHandler
        {
            set { _sequencedAvatarController.SequenceCompleteHandler = value; }
        }

        public GameObject InputDrivenAvatar;
        public GameObject SequencedAvatar;


        private void Awake()
        {
            _sequencedAvatarTransform = SequencedAvatar.GetComponent<Transform>();
            _sequencedAvatarAnimator = SequencedAvatar.GetComponent<Animator>();
            _sequencedAvatarController = SequencedAvatar.GetComponent<SequencedPlayer>();

            _basicShotPool = transform.FindChild("Kev Basic Shots").GetComponent<KevShotPool>();

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
            _inputDrivenAvatarTransform = InputDrivenAvatar.GetComponent<Transform>();
            _inputDrivenAvatarTransform.position = CurrentGame.GameData.RestartPoint;
            _inputDrivenAvatarTransform.localScale = CurrentGame.GameData.RestartScale;

            SequencedAvatar.SetActive(false);
            InputDrivenAvatar.SetActive(true);

            _basicShotPool.CanShoot = true;

            // TODO: "Let's rock!" sequence
        }

        public void EnterGate(GateType gateBeingEntered)
        {
            _gateBeingEntered = gateBeingEntered;

            CurrentGame.GameData.TimerIsFrozen = true;
            SwitchToSequencedAvatar();
            _sequencedAvatarController.SequenceCompleteHandler = CompleteGateEntrySequence;
            _sequencedAvatarAnimator.SetBool("EnteringGate", true);
        }

        public void CompleteGateEntrySequence()
        {
            if (_gateBeingEntered == GateType.Exit) 
            { 
                CurrentGame.GameData.GameplayState = GameplayState.LevelComplete;
            }
        }

        public const int Player_Physics_Layer_Index = 1024;
    }
}