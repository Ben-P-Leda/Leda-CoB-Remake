using UnityEngine;

namespace Gameplay.Shared.Scripts.Player
{
    public class ExtinguisherSpray : MonoBehaviour
    {
        private Transform _transform;
        private Transform _parentTransform;
        private ParticleSystem _particleSystem;

        private void Awake()
        {
            _transform = transform;
            _parentTransform = _transform.parent;
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void OnEnable()
        {
            _transform.rotation = Quaternion.Euler(270.0f + (80.0f * Mathf.Sign(_transform.position.x - _parentTransform.position.x)), 90.0f, 0.0f);
        }
    }
}