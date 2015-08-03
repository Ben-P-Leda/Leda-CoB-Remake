using UnityEngine;

using Shared.Scripts;
using Gameplay.Shared.Scripts.Status_Display;

namespace Gameplay.Normal.Status_Display
{
    public class KeyDisplay : DisplayBase
    {
        protected override void Start()
        {
            base.Start();

            Rect displayArea = new Rect(
                Screen.width - ((BackgroundTexture.width + Horizontal_Margin) * Scaling),
                Screen.height - ((BackgroundTexture.height + Vertical_Margin) * Scaling),
                BackgroundTexture.width * Scaling,
                BackgroundTexture.height * Scaling);

            DisplayArea = displayArea;
            TextArea = new Rect(0, 0, 0, 0);
        }

        protected override void OnGUI()
        {
            if (CurrentGame.GameData.CarryingKey) { base.OnGUI(); }
        }

        private const float Horizontal_Margin = 200.0f;
        private const float Vertical_Margin = 19.0f;
    }
}