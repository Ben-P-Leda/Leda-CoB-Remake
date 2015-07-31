using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Status_Display
{
    public class Toolbox : DisplayBase
    {
        private Rect[] _toolCountAreas;

        protected override void Start()
        {
            base.Start();

            Rect displayArea = new Rect(
                Margin * Scaling,
                Screen.height - ((BackgroundTexture.height + Margin) * Scaling),
                BackgroundTexture.width * Scaling,
                BackgroundTexture.height * Scaling);

            DisplayArea = displayArea;

            _toolCountAreas = new Rect[CurrentGame.GameData.ToolCounts.Length];
            for (int i=0; i < _toolCountAreas.Length; i++)
            {
                _toolCountAreas[i] = new Rect(
                    displayArea.x + ((Text_Horizontal_Margin + (Slot_Width * i)) * Scaling),
                    displayArea.y + (Text_Vertical_Offset * Scaling),
                    Slot_Width * Scaling,
                    displayArea.height * Scaling);
            }
        }

        private void Update()
        {
        }

        protected override void DrawText()
        {
            TextAlignment = TextAnchor.UpperLeft;

            for (int i=0; i<_toolCountAreas.Length; i++)
            {
                GUI.Label(_toolCountAreas[i], CurrentGame.GameData.ToolCounts[i].ToString(), GuiStyle);
            }
        }

        private float Text_Horizontal_Margin = 64.0f;
        private float Slot_Width = 100.0f;
        private float Text_Vertical_Offset = 30.0f;
    }
}