using UnityEngine;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class Type11Movement : HorizontalMovementBetweenLimits
    {
        private Transform _jetTrailTransform;
        private ParticleSystem _jetTrailParticles;

        protected override void Awake()
        {
            base.Awake();

            _jetTrailTransform = transform.GetChild(0);
            _jetTrailParticles = _jetTrailTransform.GetComponent<ParticleSystem>();
        }

        protected override void Update()
        {
            base.Update();

            _jetTrailTransform.rotation = Quaternion.Euler(0.0f, -Mathf.Sign(HorizontalSpeed) * 90.0f, 0.0f);
        }
    }
}