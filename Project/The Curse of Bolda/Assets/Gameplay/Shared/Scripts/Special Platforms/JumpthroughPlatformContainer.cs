using UnityEngine;

namespace Gameplay.Shared.Scripts.Special_Platforms
{
    public class JumpthroughPlatformContainer
    {
        public Transform Transform { get; private set; }
        public GameObject GameObject { get; private set; }

        public JumpthroughPlatformContainer(Transform transform, GameObject gameObject)
        {
            Transform = transform;
            GameObject = gameObject;
        }
    }
}