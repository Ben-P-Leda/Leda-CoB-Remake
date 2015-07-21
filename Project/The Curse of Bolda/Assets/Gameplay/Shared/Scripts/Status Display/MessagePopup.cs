using UnityEngine;

namespace Gameplay.Shared.Scripts.Status_Display
{
    public class MessagePopup : DisplayBase
    {
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
            Text = "Here is a long piece of text which I am using to test the word wrap on this...";
        }

        private const float Background_Horizontal_Margin = 60.0f;
        private const float Background_Vertical_Margin = 135.0f;
        private const float Text_Horizontal_Margin = 16.0f;
        private const float Text_Vertical_Margin = 13.0f;
    }
}