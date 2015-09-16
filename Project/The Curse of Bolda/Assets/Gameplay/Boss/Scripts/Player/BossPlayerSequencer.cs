using UnityEngine;

using Shared.Scripts;

using Gameplay.Shared.Scripts.Player;

namespace Gameplay.Boss.Scripts.Player
{
    public class BossPlayerSequencer : PlayerSequencer
    {
        private PushScrollTracker _pushScrollTrackerController;
        private Transform _pushScrollTrackerTransform;
        private BossPlayerController _playerController;

        protected override void Awake()
        {
            base.Awake();

            _pushScrollTrackerTransform = transform.FindChild("Push Scroll Tracker").transform;
            _pushScrollTrackerController = _pushScrollTrackerTransform.GetComponent<PushScrollTracker>();
            _playerController = InputDrivenAvatar.GetComponent<BossPlayerController>();
        }

        public override void StartNewLife()
        {
            base.StartNewLife();

            _cameraController.TransformToTrack = _pushScrollTrackerTransform;
            _pushScrollTrackerController.LockToPlayerPosition();
            _playerController.PushScrollActive = false;
        }

        public void ActivatePushScrolling()
        {
            _pushScrollTrackerController.Activate();
            _playerController.PushScrollActive = true;
        }

        public override void StartDeathSequence(PlayerDeathSequence deathSequence, SequencedPlayer.PlayerSequenceCompleteCallback sequenceCompleteCallback)
        {
            _playerController.PushScrollActive = false;
            _pushScrollTrackerController.Deactivate();

            base.StartDeathSequence(deathSequence, sequenceCompleteCallback);
        }
    }
}
