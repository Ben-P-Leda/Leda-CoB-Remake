using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class FireBehaviour : BasicBehaviour
    {
        private bool _extinguished;
        private ParticleSystem _particleSystem;
        private SpriteRenderer _spriteRenderer;
        private Color _transparent;

        protected override void Awake()
        {
            base.Awake();

            _particleSystem = GetComponent<ParticleSystem>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _transparent = new Color();
            _extinguished = false;
        }

        private void Update()
        {
            if (_extinguished)
            {
                float toolDurationFraction = CurrentGame.GameData.ToolActiveTimeRemaining / Constants.Fire_Extinguisher_Duration;
                _particleSystem.startLifetime = toolDurationFraction;
                _spriteRenderer.color = Color.Lerp(_transparent, Color.white, toolDurationFraction);

                if (CurrentGame.GameData.ToolActiveTimeRemaining <= 0.0f) { GameObject.SetActive(false); }
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collider)
        {
            base.OnTriggerEnter2D(collider);

            if (collider.tag == "Extinguisher Spray") { _extinguished = true; }
        }
    }
}
