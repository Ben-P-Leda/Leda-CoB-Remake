using UnityEngine;

namespace Gameplay.Boss.Scripts.Environment
{
    public class ArenaBoundaries : MonoBehaviour
    {
        private float _halfWidth = 0.0f;
        private Transform _leftEdgeTransform;
        private Transform _rightEdgeTransform;

        public void Awake()
        {
            if (_halfWidth <= 0.0f) { InitializeComponents(); }

            // TODO: Reset everything

            SetEdgePositions();
        }

        private void InitializeComponents()
        {
            Vector3 cameraMargins = (Camera.main.ViewportToWorldPoint(Vector3.one) - Camera.main.ViewportToWorldPoint(Vector3.zero)) * 0.5f;
            _halfWidth = cameraMargins.x;

            _leftEdgeTransform = transform.FindChild("Left Side Blocker").transform;
            _rightEdgeTransform = transform.FindChild("Right Side Blocker").transform;
        }

        private void SetEdgePositions()
        {
            Vector3 cameraPosition = Camera.main.transform.position;

            _leftEdgeTransform.position = new Vector3(cameraPosition.x - _halfWidth, cameraPosition.y, _leftEdgeTransform.position.z);
            _rightEdgeTransform.position = new Vector3(cameraPosition.x + _halfWidth, cameraPosition.y, _rightEdgeTransform.position.z);
        }
    }
}
