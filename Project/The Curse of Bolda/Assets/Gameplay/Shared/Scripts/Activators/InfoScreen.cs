using UnityEngine;

namespace Gameplay.Shared.Scripts.Activators
{
    public class InfoScreen : MonoBehaviour
    {
        private bool _isActive;
        private bool _canBeToggled;
        private SpriteRenderer _renderer;

        public Sprite OffSprite;
        public Sprite OnSprite;

        public string Message;

        private void Awake()
        {
            _renderer = transform.GetComponent<SpriteRenderer>();
            _renderer.sprite = OffSprite;

            _canBeToggled = false;
            _isActive = false;
        }

        private void Update()
        {
            if ((Input.GetKeyDown(KeyCode.S)) && (_canBeToggled))
            {
                _isActive = !_isActive;
                _renderer.sprite = (_isActive ? OnSprite : OffSprite);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            _canBeToggled = true;
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            _canBeToggled = false;
        }
    }
}