using UnityEngine;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class PlayerFollowEyeMovement : MonoBehaviour
    {
        private Transform _transform;
        private Transform _parentTransform;
        private Transform _playerTransform;

        public GameObject Player;

        private void Awake()
        {
            _transform = transform;
            _parentTransform = _transform.parent.transform;
            _playerTransform = Player.transform;
        }

        private void Update()
        {
            Vector2 lineToPlayer = _playerTransform.position - _parentTransform.position;
            lineToPlayer.Normalize();

            _transform.localPosition = new Vector3(
                (lineToPlayer.x * Offset_X_Modifier) * _parentTransform.localScale.x,
                lineToPlayer.y * Offset_Y_Modifier, 
                _transform.position.y);
        }

        private const float Offset_X_Modifier = 0.025f;
        private const float Offset_Y_Modifier = 0.02f;
    }
}