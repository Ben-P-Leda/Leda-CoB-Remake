using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Switches
{
    public class RestartPoint : SwitchBase
    {
        public delegate void ActivationCallback(string objectName);

        private GameObject _glow;
        private string _name;

        public ActivationCallback ActivationHandler {  private get; set;}

        protected override void Awake()
        {
            base.Awake();

            _glow = transform.FindChild("Restart Point Glow").gameObject;
            _name = transform.name;
        }

        protected override void AttemptStateToggle()
        {
            if (!IsActive) { base.AttemptStateToggle(); }
        }

        protected override void SetActive(bool isActive)
        {
            base.SetActive(isActive);
            _glow.SetActive(isActive);

            if (isActive) { ActivationHandler(_name);}
        }

        public void Reset()
        {
            SetActive(false);
        }
    }
}