using UnityEngine;

namespace Gameplay.Shared.Scripts.Shots
{
    public class KevShot : MonoBehaviour
    {
        private Transform _transform;
        private GameObject _gameObject;
        private Rigidbody2D _rigidBody2D;
        private ParticleSystem _particleSystem;

        public bool IsActive { get { return _gameObject.activeInHierarchy; } }

        public int DamageValue;

        private void Awake()
        {
            _transform = transform;
            _gameObject = gameObject;
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _particleSystem = GetComponent<ParticleSystem>();

            _particleSystem.Stop();
        }

        public void Activate(Vector2 startPosition, float direction)
        {
            _gameObject.SetActive(true);

            _transform.position = new Vector3(
                startPosition.x + (Horizontal_Offset * direction), startPosition.y + Vertical_Offset, _transform.position.z);
            _transform.rotation = Quaternion.Euler(0.0f, -direction * 90.0f, 0.0f);
            _rigidBody2D.velocity = new Vector2(Speed * direction, 0.0f);

            _particleSystem.Play();
        }

        private void Update()
        {
            if (!_particleSystem.IsAlive()) { _gameObject.SetActive(false); }
        }

        public void Deactivate()
        {
            _particleSystem.Stop();
        }

        private const float Speed = 7.0f;
        private const float Vertical_Offset = 0.1f;
        private const float Horizontal_Offset = 0.15f;
    }
}