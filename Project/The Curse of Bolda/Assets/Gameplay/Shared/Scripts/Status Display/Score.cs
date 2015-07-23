using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Status_Display
{
    public class Score : DisplayBase
    {
        protected override void Start()
        {
            base.Start();

            Rect displayArea = new Rect(
                (Screen.width - (BackgroundTexture.width * Scaling)) * 0.5f,
                0.0f,
                BackgroundTexture.width * Scaling,
                BackgroundTexture.height * Scaling);

            DisplayArea = displayArea;
            TextArea = new Rect(displayArea.x, displayArea.y + Text_Vertical_Offset, displayArea.width, displayArea.height - Text_Vertical_Offset);
            Text = "0";
        }

        public void Refresh()
        {
            Text = CurrentGame.GameData.Score.ToString();
        }

        private float Text_Vertical_Offset = 10.0f;
    }
}