using UnityEngine;

using System.Collections.Generic;

namespace Gameplay.Normal.Scripts.Player_Control
{
    public class WarpTracker : MonoBehaviour
    {
        private Transform _transform = null;
        private Rigidbody2D _rigidBody2D = null;
        private GameObject _gameObject = null;

        private Vector2 _targetPosition;

        public List<GameObject> Gates;

        public void Reset()
        {
            if (_gameObject == null)
            {
                _gameObject = gameObject;
                _transform = transform;
                _rigidBody2D = GetComponent<Rigidbody2D>();
            }

            _gameObject.SetActive(false);
        }

        public void Activate(Vector2 startingGatePosition)
        {
            _gameObject.SetActive(true);
            Vector3 targetGatePosition = GetTargetGate(startingGatePosition);
            SetTrajectory(startingGatePosition, targetGatePosition);
        }

        private Vector3 GetTargetGate(Vector2 startingGatePosition)
        {
            int gateIndex = Random.Range(0, Gates.Count);
            if ((Gates[gateIndex].transform.position.x == startingGatePosition.x) && (Gates[gateIndex].transform.position.y == startingGatePosition.y))
            {
                gateIndex = (gateIndex + 1) % Gates.Count;
            }

            return Gates[gateIndex].transform.position;
        }

        private void SetTrajectory(Vector2 startPosition, Vector3 targetPosition)
        {
            _transform.position = new Vector3(startPosition.x, startPosition.y, _transform.position.z);
            _targetPosition = new Vector3(targetPosition.x, targetPosition.y);

            Vector2 trajectory = new Vector2(targetPosition.x - startPosition.x, targetPosition.y - startPosition.y);
            trajectory.Normalize();

            _rigidBody2D.velocity = trajectory * Movement_Speed;
        }

        private const float Movement_Speed = 2.0f;
    }
}