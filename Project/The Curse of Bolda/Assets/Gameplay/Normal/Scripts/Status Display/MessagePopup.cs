using UnityEngine;

using Gameplay.Shared.Scripts.Status_Display;

namespace Gameplay.Normal.Scripts.Status_Display
{
    public class MessagePopup : DisplayBase
    {
        private bool _isDisplaying;
        private int _updatesBeforeDeactivationAllowed;

        protected override void Start()
        {
            base.Start();

            Rect displayArea = new Rect(
                Screen.width - ((BackgroundTexture.width + Background_Horizontal_Margin) * Scaling),
                Background_Vertical_Margin * Scaling,
                BackgroundTexture.width * Scaling,
                BackgroundTexture.height * Scaling);

            DisplayArea = displayArea;
            TextArea = new Rect(
                displayArea.x + (Text_Horizontal_Margin * Scaling), 
                displayArea.y + (Text_Vertical_Margin * Scaling),
                displayArea.width - (Text_Horizontal_Margin * 2.0f * Scaling), 
                displayArea.height - (Text_Vertical_Margin * 2.0f * Scaling));

            TextAlignment = TextAnchor.UpperLeft;

            _isDisplaying = false;
        }

        public void Activate(string textToDisplay)
        {
            Text = textToDisplay;
            _isDisplaying = true;
            _updatesBeforeDeactivationAllowed = Deactivation_Update_Count;
            Time.timeScale = 0.0f;
        }

        protected override void OnGUI()
        {
            if (_isDisplaying) 
            {
                base.OnGUI(); 
            }
        }

        private void Update()
        {
            if ((_isDisplaying) && (--_updatesBeforeDeactivationAllowed <= 0.0f) && (Input.anyKeyDown))
            {
                _isDisplaying = false;
                Time.timeScale = 1.0f;
            }
        }

        private const float Background_Horizontal_Margin = 60.0f;
        private const float Background_Vertical_Margin = 135.0f;
        private const float Text_Horizontal_Margin = 16.0f;
        private const float Text_Vertical_Margin = 13.0f;
        private const int Deactivation_Update_Count = 10;
    }
}