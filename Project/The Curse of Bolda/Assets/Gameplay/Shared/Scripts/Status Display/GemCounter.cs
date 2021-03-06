﻿using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Status_Display
{
    public class GemCounter : DisplayBase
    {
        protected override void Start()
        {
            base.Start();

            Rect displayArea = new Rect(
                Screen.width - ((BackgroundTexture.width + Margin) * Scaling),
                Screen.height - ((BackgroundTexture.height + Margin) * Scaling),
                BackgroundTexture.width * Scaling,
                BackgroundTexture.height * Scaling);

            DisplayArea = displayArea;
            TextArea = new Rect(displayArea.x, displayArea.y, displayArea.width - displayArea.height, displayArea.height);
            Text = "0";
        }

        public void Refresh()
        {
            Text = CurrentGame.GameData.GemsCollected.ToString();
        }
    }
}