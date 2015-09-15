﻿using UnityEngine;

using Gameplay.Shared.Scripts.Player;

namespace Gameplay.Boss.Scripts.Player
{
    public class PushScrollTracker : MonoBehaviour
    {
        private Transform _playerTransform;
        private Rigidbody2D _rigidbody2D;
        private Transform _transform;
        private float _leftSideLimit;

        public GameObject InputDrivenAvatar;

        private void Awake()
        {
            _transform = transform;
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _playerTransform = InputDrivenAvatar.transform;
        }

        private void Start()
        {
            Vector3 cameraMargins = (Camera.main.ViewportToWorldPoint(Vector3.one) - Camera.main.ViewportToWorldPoint(Vector3.zero)) * 0.5f;
            _leftSideLimit = cameraMargins.x;
        }

        public void LockToPlayerPosition()
        {
            _transform.position = new Vector3(Mathf.Max(_playerTransform.position.x, _leftSideLimit), _playerTransform.position.y, _transform.position.z);
            _rigidbody2D.velocity = Vector2.zero;
        }

        public void Activate()
        {
            _rigidbody2D.velocity = new Vector2(InputDrivenPlayer.Walk_Speed, 0.0f);
        }
    }
}
