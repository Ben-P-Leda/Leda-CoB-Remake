using UnityEngine;

namespace Gameplay.Boss.Scripts.Player
{
    public class PushScrollTracker : MonoBehaviour
    {
        private Transform _playerTransform;
        private Rigidbody2D _rigidbody2D;
        private Transform _transform;

        public GameObject InputDrivenAvatar;

        private void Awake()
        {
            _transform = transform;
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _playerTransform = InputDrivenAvatar.transform;
        }

        public void LockToPlayerPosition()
        {
            _transform.position = new Vector3(_playerTransform.position.x, _playerTransform.position.y, _transform.position.z);
            _rigidbody2D.velocity = Vector2.zero;

            Diagnostics.DiagnosticsDisplay.SetDiagnostic("x", _playerTransform.position.x.ToString());
            Diagnostics.DiagnosticsDisplay.SetDiagnostic("y", Camera.main.transform.position.x.ToString());
        }

        public void Activate()
        {
            _rigidbody2D.velocity = new Vector2(Speed, 0.0f);
        }

        private const float Speed = 2.0f;
    }
}
