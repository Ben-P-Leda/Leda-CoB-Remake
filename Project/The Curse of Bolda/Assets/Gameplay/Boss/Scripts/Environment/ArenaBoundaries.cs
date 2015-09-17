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
            _leftEdgeTransform.localPosition = new Vector3(- _halfWidth, 0.0f, 0.0f);
            _rightEdgeTransform.position = new Vector3(_halfWidth, 0.0f, 0.0f);
        }
    }
}
