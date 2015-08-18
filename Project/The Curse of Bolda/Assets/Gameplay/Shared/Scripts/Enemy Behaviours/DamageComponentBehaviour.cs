using UnityEngine;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class DamageComponentBehaviour : BasicBehaviour
    {
        private GameObject _physicsComponent;

        protected override void Awake()
        {
            base.Awake();

            _physicsComponent = transform.parent.gameObject;
        }

        protected override void HandleCollisionWithPlayerShot(int hitPointDelta)
        {
            base.HandleCollisionWithPlayerShot(hitPointDelta);

            if (HitPoints < 1) { _physicsComponent.SetActive(false); }
        }
    }
}
