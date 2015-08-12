using UnityEngine;

namespace Gameplay.Shared.Scripts.Effects
{
    public class SmokeCloud : MonoBehaviour
    {
        private Transform _transform = null;
        private ParticleSystem _particleSystem = null;

        public bool IsActive { get { return _particleSystem.isPlaying; } }

        private void Awake()
        {
            _transform = transform;
            _particleSystem = GetComponent<ParticleSystem>();
        }

        public void Activate(Vector2 position)
        {
            _transform.position = position;
            _particleSystem.Play();
        }
    }
}