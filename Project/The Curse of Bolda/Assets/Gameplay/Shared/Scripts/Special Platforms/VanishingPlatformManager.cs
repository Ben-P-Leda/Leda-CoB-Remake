using UnityEngine;

namespace Gameplay.Shared.Scripts.Special_Platforms
{
    public class VanishingPlatformManager : JumpthroughPlatformManager
    {
        protected override void Awake()
        {
            base.Awake();

            for (int i=0; i<Platforms.Length; i++)
            {
                BoxCollider2D physicsCollider = Platforms[i].GameObject.GetComponent<BoxCollider2D>();

                BoxCollider2D triggerCollider = Platforms[i].GameObject.AddComponent<BoxCollider2D>();
                triggerCollider.offset = physicsCollider.offset;
                triggerCollider.isTrigger = true;

                Platforms[i].GameObject.AddComponent<VanishingPlatform>();
            }
        }
    }
}