using UnityEngine;

namespace Gameplay.Shared.Scripts.Environment
{
    public class BackgroundLayer : MonoBehaviour
    {
        private Transform _transform;
        private Vector2 _movementExtent;

        private void Awake()
        {
            _transform = transform;
        }

        public void CalculateMovementExtent(Vector2 cameraMargins)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            _movementExtent = new Vector2(
                spriteRenderer.sprite.bounds.extents.x - cameraMargins.x,
                spriteRenderer.sprite.bounds.extents.y - cameraMargins.y);
        }

        public void PositionRelativeToCamera(Vector3 cameraPosition, Vector2 offset)
        {
            _transform.position = new Vector3(
                cameraPosition.x - (_movementExtent.x * offset.x),
                cameraPosition.y - (_movementExtent.y * offset.y),
                _transform.position.z);
        }
    }
}
