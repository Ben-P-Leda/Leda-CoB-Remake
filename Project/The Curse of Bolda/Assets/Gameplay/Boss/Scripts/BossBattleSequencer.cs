using UnityEngine;

namespace Assets.Gameplay.Boss.Scripts
{
    public class BossBattleSequencer : MonoBehaviour
    {
        private Transform _transform;
        private Transform _cameraTransform;

        private void Awake()
        {
            _transform = transform;
            _cameraTransform = Camera.main.transform;

            UpdatePosition();
        }

        private void Update()
        {
            UpdatePosition();
        }

        private void UpdatePosition()
        {
            _transform.position = new Vector3(_cameraTransform.position.x, _cameraTransform.position.y, _transform.position.z);
        }
    }
}
