using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Status_Display
{
    public class Toolbox : DisplayBase
    {
        protected override void Start()
        {
            base.Start();

            Rect displayArea = new Rect(
                Margin * Scaling,
                Screen.height - ((BackgroundTexture.height + Margin) * Scaling),
                BackgroundTexture.width * Scaling,
                BackgroundTexture.height * Scaling);

            DisplayArea = displayArea;
            TextArea = new Rect(0,0,0,0);
        }

        private void Update()
        {
        }

        protected override void DrawText()
        {
        }
    }
}