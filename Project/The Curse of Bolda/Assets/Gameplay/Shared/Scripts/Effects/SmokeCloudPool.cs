using UnityEngine;
using Gameplay.Shared.Scripts.Generic;

namespace Gameplay.Shared.Scripts.Effects
{
    public sealed class SmokeCloudPool : GenericObjectPool<PooledParticleEffect>
    {
        private static SmokeCloudPool _instance = null;
        public static PooledParticleEffect ActivateSmokeCloud(Vector2 position) { return _instance.ActivateCloudAt(position); }

        protected override void Awake()
        {
            base.Awake();

            if (_instance == null) { _instance = this; }
        }

        protected override bool ObjectIsAvailable(PooledParticleEffect objectToCheck)
        {
            return !objectToCheck.IsActive;
        }

        private PooledParticleEffect ActivateCloudAt(Vector2 position)
        {
            PooledParticleEffect cloud = GetFirstAvailableObject();
            if (cloud != null) { cloud.Activate(position); }

            return cloud;
        }
    }
}