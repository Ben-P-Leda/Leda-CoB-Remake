﻿using UnityEngine;

using Gameplay.Shared.Scripts.Generic;

namespace Gameplay.Shared.Scripts.Switches
{
    public class SwitchBase : MonoBehaviour, IChangesStateOnNewLifeStart
    {
        private bool _isActive;
        private bool _canBeToggled;
        private SpriteRenderer _renderer;

        protected bool IsActive { get { return _isActive; } }

        public Sprite OffSprite;
        public Sprite OnSprite;

        protected virtual void Awake()
        {
            _renderer = transform.GetComponent<SpriteRenderer>();
            _renderer.sprite = OffSprite;

            _canBeToggled = false;
            _isActive = false;
        }

        private void Update()
        {
            if ((Input.GetKeyDown(KeyCode.S)) && (_canBeToggled)) { AttemptStateToggle(); }
        }

        protected virtual void AttemptStateToggle()
        {
            SetActive(!_isActive);
        }

        protected virtual void SetActive(bool isActive) 
        {
            _isActive = isActive;
            _renderer.sprite = (_isActive ? OnSprite : OffSprite);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Kev") 
            { 
                _canBeToggled = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.tag == "Kev") 
            { 
                _canBeToggled = false;
            }
        }

        public void SetForPlayerNewLifeStart()
        {
            _canBeToggled = false;
        }
    }
}
