﻿using UnityEngine;

namespace Gameplay.Shared.Scripts.Special_Platforms
{
    public class VanishingPlatform : MonoBehaviour
    {
        private Transform _transform;
        private SpriteRenderer _renderer;
        private BoxCollider2D[] _colliders;

        private void Awake()
        {
            _transform = transform;
            _renderer = GetComponent<SpriteRenderer>();
            _colliders = GetComponents<BoxCollider2D>();

            SetActiveState(true);
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            if ((collider.tag == "Kev") && (_transform.localScale.x > 0.0f))
            {
                _transform.localScale = new Vector3(Mathf.Clamp01(_transform.localScale.x - Shrink_Rate), 1.0f, 1.0f);

                if (_transform.localScale.x <= Vanishing_Threshold) 
                { 
                    SetActiveState(false);
                }
            }
        }

        private void SetActiveState(bool isActive)
        {
            // TODO: Effect if desired

            _renderer.enabled = isActive;
            for (int i = 0; i < _colliders.Length; i++) { _colliders[i].enabled = isActive; }

            _transform.localScale = isActive ? Vector3.one : Vector3.zero;
        }

        private void OnEnable()
        {
            SetActiveState(true);
        }

        private const float Shrink_Rate = 0.02f;
        private const float Vanishing_Threshold = 0.085f;
    }
}