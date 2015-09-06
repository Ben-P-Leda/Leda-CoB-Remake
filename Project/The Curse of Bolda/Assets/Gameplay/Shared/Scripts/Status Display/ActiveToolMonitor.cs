using System;
using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Status_Display
{
    public class ActiveToolMonitor : ScreenRelativeComponent
    {
        private Rect _displayArea;

        private Rect _timerArea;
        private Vector2 _timerCenter;
        private float _totalTime;

        public Texture2D BackgroundTexture;
        public Texture2D TimerColourHalfTexture;
        public Texture2D TimerBlackHalfTexture;
        public Texture2D ToolsTexture;
        
        private ToolType _activeTool;
        private DisplayOption _displayOption;
        private Rect _activeToolTextureArea;

        private void Awake()
        {
            _activeTool = ToolType.None;
        }

        protected override void Start()
        {
            base.Start();
            _displayArea = new Rect(
                Screen.width - ((BackgroundTexture.width + Horizontal_Margin) * Scaling),
                Vertical_Margin * Scaling,
                BackgroundTexture.width * Scaling,
                BackgroundTexture.height * Scaling);

            _timerCenter = new Vector2(
                _displayArea.x + ((_displayArea.width + Scaling) * 0.5f), 
                _displayArea.y + ((_displayArea.height + Scaling) * 0.5f));

            _timerArea = new Rect(
                _timerCenter.x - (TimerColourHalfTexture.width * Scaling),
                _timerCenter.y - ((TimerColourHalfTexture.height * 0.5f) * Scaling),
                TimerColourHalfTexture.width * Scaling,
                TimerColourHalfTexture.height * Scaling);
        }

        protected virtual void OnGUI()
        {
            if (_displayOption != DisplayOption.DoNotDisplay)
            {
                GUI.DrawTexture(_displayArea, BackgroundTexture);
                GUI.DrawTextureWithTexCoords(_displayArea, ToolsTexture, _activeToolTextureArea);

                if (_displayOption == DisplayOption.DisplayTimer)
                {
                    GUI.DrawTexture(_timerArea, TimerColourHalfTexture);

                    GUIUtility.RotateAroundPivot(180.0f, _timerCenter);
                    GUI.DrawTexture(_timerArea, TimerBlackHalfTexture);

                    float rotation = (540.0f - (360.0f * (CurrentGame.GameData.ActiveToolTimeRemaining / _totalTime))) % 360.0f;
                    if (rotation <= 180.0f)
                    {
                        GUIUtility.RotateAroundPivot(rotation, _timerCenter);
                        GUI.DrawTexture(_timerArea, TimerBlackHalfTexture);
                    }
                    else
                    {
                        GUIUtility.RotateAroundPivot(rotation - 180.0f, _timerCenter);
                        GUI.DrawTexture(_timerArea, TimerColourHalfTexture);
                    }
                }
            }
        }

        private void Update()
        {
            if (DisplayPropertiesRequireUpdating())
            {
                UpdateDisplayProperties(CurrentGame.GameData.ActiveTool);
            }

            _activeTool = CurrentGame.GameData.ActiveTool;
        }

        private bool DisplayPropertiesRequireUpdating()
        {
            if ((CurrentGame.ToolIsActive) && (_activeTool == ToolType.None)) { return true; }
            if ((!CurrentGame.ToolIsActive) && (_activeTool != ToolType.None)) { return true; }

            return false;
        }

        private void UpdateDisplayProperties(ToolType toolType)
        {
            if (CurrentGame.ToolIsActive)
            {
                SetDisplayValues(toolType);
                SetDisplayTexture(toolType);
            }
            else
            {
                _displayOption = DisplayOption.DoNotDisplay;
            }
        }

        private void SetDisplayValues(ToolType toolType)
        {
            switch (toolType)
            {
                case ToolType.Invincibility: _displayOption = DisplayOption.DisplayTimer; break;
                case ToolType.Jetpack: _displayOption = DisplayOption.DisplayTimer; _totalTime = Constants.Jetpack_Duration; break;
                case ToolType.SuperJump: _displayOption = DisplayOption.DisplayOneShot; break;
                case ToolType.FirepowerUp: _displayOption = DisplayOption.DisplayTimer; break;
                case ToolType.Pickaxe: _displayOption = DisplayOption.DisplayTimer; break;
                case ToolType.FireExtinguisher: _displayOption = DisplayOption.DoNotDisplay; break;
            }
        }

        private void SetDisplayTexture(ToolType toolType)
        {
            int toolIndex = Convert.ToInt32(toolType);
            int x = toolIndex % 3;
            int y = toolIndex / 3;
            _activeToolTextureArea = new Rect(x * 0.333f, 0.5f - (y * 0.5f), 0.333f, 0.5f);
        }

        private enum DisplayOption
        {
            DoNotDisplay,
            DisplayOneShot,
            DisplayTimer
        }

        protected const float Horizontal_Margin = 10.0f;
        protected const float Vertical_Margin = 90.0f;

        private const float Font_Size = 26.0f;
    }
}