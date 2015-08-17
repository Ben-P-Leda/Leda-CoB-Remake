using UnityEngine;

namespace Gameplay.Normal.Scripts.Field_Objects
{
    public class ExitGate : MonoBehaviour
    {
        private Sprite _sprite;
        private SpriteRenderer _spriteRenderer;
        private Rect _fullSizeRect;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _sprite = _spriteRenderer.sprite;
            _fullSizeRect = _sprite.textureRect;
        }
    }
}