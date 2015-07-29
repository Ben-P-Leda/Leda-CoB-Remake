using UnityEngine;

using Gameplay.Shared.Scripts.Switches;
using Gameplay.Normal.Scripts.Status_Display;

namespace Gameplay.Normal.Scripts.Switches
{
    public class InfoScreen : ActivatorBase
    {
        private MessagePopup _messageBox;

        public GameObject MessageBoxObject;

        public string Message;

        protected override void Awake()
        {
            base.Awake();
            _messageBox = MessageBoxObject.GetComponent<MessagePopup>();
        }

        protected override void SetActive(bool isActive)
        {
            base.SetActive(isActive);

            _messageBox.Activate(Message);
        }
    }
}