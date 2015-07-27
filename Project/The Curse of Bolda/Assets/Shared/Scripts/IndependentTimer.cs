using UnityEngine;

using System;

namespace Shared.Scripts
{
    public class IndependentTimer : MonoBehaviour
    {
        private long _creationTimeTicks;
        private long _currentUpdateTicks;
        private long _lastUpdateTicks;

        public float TotalElapsed { get { return (_currentUpdateTicks - _creationTimeTicks) * Seconds_Modifier; } }
        public float ElapsedSinceLastUpdate { get { return (_currentUpdateTicks - _lastUpdateTicks) * Seconds_Modifier; } }

        private void Awake()
        {
            _creationTimeTicks = DateTime.Now.Ticks;
            _currentUpdateTicks = _creationTimeTicks;
            _lastUpdateTicks = _creationTimeTicks;
        }

        private void Update()
        {
            _lastUpdateTicks = _currentUpdateTicks;
            _currentUpdateTicks = DateTime.Now.Ticks;
        }

        private float Seconds_Modifier = 0.0000001f;
    }
}
