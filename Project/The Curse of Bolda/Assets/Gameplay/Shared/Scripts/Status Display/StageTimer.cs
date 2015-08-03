using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Status_Display
{
    public class StageTimer : DisplayBase
    {
        private Rect _minutesTextArea;
        private Rect _secondsTextArea;

        protected override void Start()
        {
            base.Start();

            Rect backgroundArea = new Rect(
                Screen.width - ((BackgroundTexture.width + Margin) * Scaling),
                Margin * Scaling,
                BackgroundTexture.width * Scaling,
                BackgroundTexture.height * Scaling);

            _secondsTextArea = new Rect(
                backgroundArea.x + (Split_Margin * Scaling), 
                backgroundArea.y,
                backgroundArea.width - backgroundArea.height, 
                backgroundArea.height);

            _minutesTextArea = new Rect(
                _secondsTextArea.x - _secondsTextArea.width, 
                backgroundArea.y, 
                _secondsTextArea.width,
                backgroundArea.height);

            DisplayArea = backgroundArea;
        }

        protected override void DrawText()
        {
            string minutes = string.Format("{0}:", Mathf.FloorToInt(Mathf.Max(CurrentGame.GameData.TimeRemaining / 60.0f, 0.0f)));
            string seconds = string.Format("{0}{1}",
                Mathf.Max(CurrentGame.GameData.TimeRemaining % 60.0f) < 10.0f ? "0" : "",
                Mathf.FloorToInt(Mathf.Max(CurrentGame.GameData.TimeRemaining % 60.0f)));

            GuiStyle.alignment = TextAnchor.MiddleRight;
            GUI.Label(_minutesTextArea, minutes, GuiStyle);

            GuiStyle.alignment = TextAnchor.MiddleLeft;
            GUI.Label(_secondsTextArea, seconds, GuiStyle);
        }

        private const float Split_Margin = 40.0f;
    }
}