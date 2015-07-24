using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Status_Display
{
    public class Score : DisplayBase
    {
        private int _displayedScore;

        protected override void Start()
        {
            base.Start();

            Rect displayArea = new Rect(
                (Screen.width - (BackgroundTexture.width * Scaling)) * 0.5f,
                Margin * Scaling,
                BackgroundTexture.width * Scaling,
                BackgroundTexture.height * Scaling);

            DisplayArea = displayArea;
            TextArea = new Rect(displayArea.x, displayArea.y + Text_Vertical_Offset, displayArea.width, displayArea.height - Text_Vertical_Offset);

            _displayedScore = 0;
        }

        private void Update()
        {
            if (_displayedScore < CurrentGame.GameData.Score) { _displayedScore += 1; }
            Text = _displayedScore.ToString();
        }

        private float Text_Vertical_Offset = 15.0f;
    }
}