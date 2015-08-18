using UnityEngine;
using Gameplay.Shared.Scripts.Generic;

namespace Gameplay.Shared.Scripts.Effects
{
    public sealed class GemCollectSparklePool : GenericObjectPool<PooledParticleEffect>
    {
        private static GemCollectSparklePool _instance = null;
        public static PooledParticleEffect ActivateGemCollectSparkle(Vector2 position) { return _instance.ActivateSparkleAt(position); }

        protected override void Awake()
        {
            base.Awake();

            if (_instance == null) { _instance = this; }
        }

        protected override bool ObjectIsAvailable(PooledParticleEffect objectToCheck)
        {
            return !objectToCheck.IsActive;
        }

        private PooledParticleEffect ActivateSparkleAt(Vector2 position)
        {
            PooledParticleEffect sparkle = GetFirstAvailableObject();
            if (sparkle != null) { sparkle.Activate(position); }

            return sparkle;
        }
    }
}