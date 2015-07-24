using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Status_Display
{
    public class StageTimer : DisplayBase
    {
        private Rect _emptyArea;
        private Rect _backgroundArea;
        private Rect _minutesTextArea;
        private Rect _secondsTextArea;

        protected override void Start()
        {
            base.Start();

            _emptyArea = new Rect(0, 0, 0, 0);

            _backgroundArea = new Rect(
                Screen.width - ((BackgroundTexture.width + Margin) * Scaling),
                Margin * Scaling,
                BackgroundTexture.width * Scaling,
                BackgroundTexture.height * Scaling);

            _secondsTextArea = new Rect(
                _backgroundArea.x + (Split_Margin * Scaling), 
                _backgroundArea.y,
                _backgroundArea.width - _backgroundArea.height, 
                _backgroundArea.height);

            _minutesTextArea = new Rect(
                _secondsTextArea.x - _secondsTextArea.width, 
                _backgroundArea.y, 
                _secondsTextArea.width,
                _backgroundArea.height);
        }

        protected override void OnGUI()
        {
            DisplayArea = _backgroundArea;
            TextArea = _minutesTextArea;
            TextAlignment = TextAnchor.MiddleRight;
            Text = string.Format("{0}:", Mathf.FloorToInt(Mathf.Max(CurrentGame.GameData.TimeRemaining / 60.0f, 0.0f)));
            base.OnGUI();

            DisplayArea = _emptyArea;
            TextArea = _secondsTextArea;
            TextAlignment = TextAnchor.MiddleLeft;
            Text = string.Format("{0}{1}",
                Mathf.Max(CurrentGame.GameData.TimeRemaining % 60.0f) < 10.0f ? "0" : "",
                Mathf.FloorToInt(Mathf.Max(CurrentGame.GameData.TimeRemaining % 60.0f)));
            base.OnGUI();
        }

        private const float Split_Margin = 40.0f;
    }
}