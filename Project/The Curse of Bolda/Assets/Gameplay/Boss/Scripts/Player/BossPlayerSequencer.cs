﻿using UnityEngine;

using Shared.Scripts;

using Gameplay.Shared.Scripts.Player;

namespace Gameplay.Boss.Scripts.Player
{
    public class BossPlayerSequencer : PlayerSequencer
    {
        private PushScrollTracker _pushScrollTrackerController;
        private Transform _pushScrollTrackerTransform;
        private BossPlayerController _playerController;

        public float PushScrollPosition { get { return _pushScrollTrackerTransform.position.x; } }

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
        }

        public void ActivatePushScrolling()
        {
            _pushScrollTrackerController.Activate();
            _playerController.LevelInProgress = true;
        }

        public override void StartDeathSequence(PlayerDeathSequence deathSequence, SequencedPlayer.PlayerSequenceCompleteCallback sequenceCompleteCallback)
        {
            _playerController.LevelInProgress = false;
            _pushScrollTrackerController.Deactivate();

            base.StartDeathSequence(deathSequence, sequenceCompleteCallback);
        }

        public void StartBossBattle()
        {
            _pushScrollTrackerController.Deactivate();
            _playerController.PushScrollActive = false;
        }
    }
}
