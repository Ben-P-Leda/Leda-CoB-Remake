using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Status_Display
{
    public class ToolTimer : ScreenRelativeComponent
    {
        private Rect _displayArea;

        public Texture2D BackgroundTexture;

        protected override void Start()
        {
            base.Start();
        }

        protected virtual void OnGUI()
        {
            //GUI.DrawTexture(_displayArea, BackgroundTexture);
        }

        private void Update()
        {
            if (CurrentGame.ToolIsActive)
            {
                Diagnostics.DiagnosticsDisplay.SetDiagnostic("tooltimer", CurrentGame.GameData.ToolActiveTimeRemaining.ToString());

                CurrentGame.GameData.ToolActiveTimeRemaining -= Time.deltaTime;
            }
        }

        protected const float Margin = 10.0f;

        private const float Font_Size = 26.0f;
    }
}