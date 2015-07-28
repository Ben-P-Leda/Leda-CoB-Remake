using UnityEngine;
using Gameplay.Shared.Scripts.Status_Display;

namespace Gameplay.Shared.Scripts.Switches
{
    public class InfoScreen : ActivatorBase
    {
        private MessagePopup _messageBox;

        public GameObject MessageBoxObject;

        public string Message;

        protected override void Awake()
        {
            base.Awake();
            _messageBox = MessageBoxObject.GetComponent<Gameplay.Shared.Scripts.Status_Display.MessagePopup>();
        }

        protected override void SetActive(bool isActive)
        {
            base.SetActive(isActive);

            _messageBox.Activate(Message);
        }
    }
}