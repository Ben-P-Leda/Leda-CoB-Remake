using UnityEngine;
using Gameplay.Shared.Scripts.Generic;

namespace Gameplay.Shared.Scripts.Effects
{
    public sealed class WaterSplashPool : GenericObjectPool<PooledParticleEffect>
    {
        private static WaterSplashPool _instance = null;
        public static PooledParticleEffect ActivateWaterSplash(Vector2 position) { return _instance.ActivateWaterSplashAt(position); }

        protected override void Awake()
        {
            base.Awake();

            if (_instance == null) { _instance = this; }
        }

        protected override bool ObjectIsAvailable(PooledParticleEffect objectToCheck)
        {
            return !objectToCheck.IsActive;
        }

        private PooledParticleEffect ActivateWaterSplashAt(Vector2 position)
        {
            PooledParticleEffect waterSplash = GetFirstAvailableObject();
            if (waterSplash != null) { waterSplash.Activate(position); }

            return waterSplash;
        }
    }
}