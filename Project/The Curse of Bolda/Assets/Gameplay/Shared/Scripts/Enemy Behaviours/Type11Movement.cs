using UnityEngine;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class Type11Movement : HorizontalMovementBetweenLimits
    {
        private Transform _jetTrailTransform;
        private ParticleSystem _jetTrailParticles;
        private float _previousHorizontalSpeed;

        protected override void Awake()
        {
            base.Awake();

            _jetTrailTransform = transform.GetChild(0);
            _jetTrailParticles = _jetTrailTransform.GetComponent<ParticleSystem>();

            _previousHorizontalSpeed = HorizontalSpeed;
        }

        protected override void Update()
        {
            base.Update();

            if ((Mathf.Abs(HorizontalSpeed) >= Mathf.Abs(_previousHorizontalSpeed)) && (!_jetTrailParticles.enableEmission)) { _jetTrailParticles.enableEmission = true; }
            if ((Mathf.Abs(HorizontalSpeed) < Mathf.Abs(_previousHorizontalSpeed)) && (_jetTrailParticles.enableEmission)) { _jetTrailParticles.enableEmission = false; }

            _jetTrailTransform.rotation = Quaternion.Euler(0.0f, -Mathf.Sign(HorizontalSpeed) * 90.0f, 0.0f);

            _previousHorizontalSpeed = HorizontalSpeed;
        }
    }
}