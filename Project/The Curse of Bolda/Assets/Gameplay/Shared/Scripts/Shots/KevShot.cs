using UnityEngine;

namespace Gameplay.Shared.Scripts.Shots
{
    public class KevShot : MonoBehaviour
    {
        private Transform _transform;
        private GameObject _gameObject;
        private Rigidbody2D _rigidBody2D;
        private CircleCollider2D _collider;
        private ParticleSystem _particleSystem;

        private float _lifetimeRemaining;

        public bool IsActive { get { return _gameObject.activeInHierarchy; } }

        private void Awake()
        {
            _transform = transform;
            _gameObject = gameObject;
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CircleCollider2D>();
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
            _collider.enabled = true;
            _particleSystem.Play();

            _lifetimeRemaining = Lifespan;
        }

        private void Update()
        {
            _lifetimeRemaining -= Time.deltaTime;
            if (_lifetimeRemaining <= 0.0f) { _gameObject.SetActive(false); }
            else if ((_lifetimeRemaining < TailOff_Duration) && (_collider.enabled)) { Deactivate(); }
        }

        public void Deactivate()
        {
            _rigidBody2D.velocity = Vector2.zero;
            _collider.enabled = false;
            _particleSystem.Stop();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Deactivate();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Enemy") { Deactivate(); }
        }

        private const float Speed = 7.0f;
        private const float Vertical_Offset = 0.1f;
        private const float Horizontal_Offset = 0.15f;
        private const float Lifespan = 1.0f;
        private const float TailOff_Duration = 0.2f;
    }
}