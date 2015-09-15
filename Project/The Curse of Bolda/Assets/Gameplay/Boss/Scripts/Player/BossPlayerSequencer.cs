using UnityEngine;

using Gameplay.Shared.Scripts.Player;

namespace Gameplay.Boss.Scripts.Player
{
    public class BossPlayerSequencer : PlayerSequencer
    {
        private PushScrollTracker _pushScrollTrackerController;
        private Transform _pushScrollTrackerTransform;

        protected override void Awake()
        {
            base.Awake();

            _pushScrollTrackerTransform = transform.FindChild("Push Scroll Tracker").transform;
            _pushScrollTrackerController = _pushScrollTrackerTransform.GetComponent<PushScrollTracker>();
        }

        public override void StartNewLife()
        {
            base.StartNewLife();

            _cameraController.TransformToTrack = _pushScrollTrackerTransform;
            _pushScrollTrackerController.LockToPlayerPosition();
        }

        public void ActivatePushScrollTracker()
        {
            _pushScrollTrackerController.Activate();
        }
    }
}
