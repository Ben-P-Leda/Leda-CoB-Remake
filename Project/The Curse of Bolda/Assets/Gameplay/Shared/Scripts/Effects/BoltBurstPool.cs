using UnityEngine;
using Gameplay.Shared.Scripts.Generic;

namespace Gameplay.Shared.Scripts.Effects
{
    public sealed class BoltBurstPool : GenericObjectPool<PooledParticleEffect>
    {
        private static BoltBurstPool _instance = null;
        public static PooledParticleEffect ActivateBoltBurst(Vector2 position) { return _instance.ActivateBoltBurstAt(position); }

        protected override void Awake()
        {
            base.Awake();

            if (_instance == null) { _instance = this; }
        }

        protected override bool ObjectIsAvailable(PooledParticleEffect objectToCheck)
        {
            return !objectToCheck.IsActive;
        }

        private PooledParticleEffect ActivateBoltBurstAt(Vector2 position)
        {
            PooledParticleEffect boltBurst = GetFirstAvailableObject();
            if (boltBurst != null) { boltBurst.Activate(position); }

            return boltBurst;
        }
    }
}