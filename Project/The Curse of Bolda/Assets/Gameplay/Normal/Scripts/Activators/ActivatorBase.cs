using UnityEngine;

using System.Collections.Generic;

using Gameplay.Shared.Scripts.Switches;

namespace Gameplay.Normal.Scripts.Activators
{
    public class ActivatorBase : SwitchBase
    {
        protected bool HasBeenActivated { get; private set; }

        public List<GameObject> OneWayActivations;
        public List<GameObject> TwoWayActivations;

        protected override void Awake()
        {
            base.Awake();
            HasBeenActivated = false;
        }

        protected override void SetActive(bool isActive)
        {
            base.SetActive(isActive);

            if (!HasBeenActivated) { ToggleObjects(OneWayActivations); }
            ToggleObjects(TwoWayActivations);

            HasBeenActivated = true;
        }

        private void ToggleObjects(List<GameObject> toToggle)
        {
            for (int i = 0; i < toToggle.Count; i++)
            {
                toToggle[i].SetActive(!toToggle[i].activeInHierarchy);
            }
        }
    }
}
