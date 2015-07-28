using UnityEngine;

using System.Collections.Generic;

namespace Gameplay.Shared.Scripts.Switches
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

            if (!HasBeenActivated) { ActivateOneWayObjects(); }
            ToggleTwoWayObjects();

            HasBeenActivated = true;
        }

        private void ActivateOneWayObjects()
        {
            for (int i = 0; i < OneWayActivations.Count; i++) 
            {
                if (!OneWayActivations[i].activeInHierarchy)
                {
                    OneWayActivations[i].SetActive(true);
                }
            }
        }

        private void ToggleTwoWayObjects()
        {
            for (int i=0; i<TwoWayActivations.Count; i++)
            {
                TwoWayActivations[i].SetActive(!TwoWayActivations[i].activeInHierarchy);
            }
        }
    }
}
