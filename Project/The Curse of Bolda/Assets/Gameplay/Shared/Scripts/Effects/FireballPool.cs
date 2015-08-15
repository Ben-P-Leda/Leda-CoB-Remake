using UnityEngine;
using Gameplay.Shared.Scripts.Generic;

namespace Gameplay.Shared.Scripts.Effects
{
    public sealed class FireballPool : GenericObjectPool<PooledParticleEffect>
    {
        private static FireballPool _instance = null;
        public static PooledParticleEffect ActivateFireball(Vector2 position) { return _instance.ActivateFireballAt(position); }

        protected override void Awake()
        {
            base.Awake();

            if (_instance == null) { _instance = this; }
        }

        protected override bool ObjectIsAvailable(PooledParticleEffect objectToCheck)
        {
            return !objectToCheck.IsActive;
        }

        private PooledParticleEffect ActivateFireballAt(Vector2 position)
        {
            PooledParticleEffect fireball = GetFirstAvailableObject();
            if (fireball != null) { fireball.Activate(position); }

            return fireball;
        }
    }
}