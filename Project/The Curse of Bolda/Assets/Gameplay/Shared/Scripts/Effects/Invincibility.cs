using UnityEngine;

namespace Gameplay.Shared.Scripts.Effects
{
    public class Invincibility : MonoBehaviour
    {
        private Transform _transform;
        private float _rotation;

        private void Awake()
        {
            _transform = transform;
            _rotation = 0.0f;
        }

        private void Update()
        {
            _transform.localRotation = Quaternion.Euler(0.0f, 0.0f, _rotation);
            _rotation += 2.5f;
        }
    }
}