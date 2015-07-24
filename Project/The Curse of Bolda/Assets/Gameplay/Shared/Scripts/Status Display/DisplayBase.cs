using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Status_Display
{
    public class DisplayBase : ScreenRelativeComponent
    {
        private Rect _displayArea;
        private Rect _textArea;
        private GUIStyle _guiStyle;
        private string _text;

        public Texture2D BackgroundTexture;
        public Font Font;

        protected GUIStyle GuiStyle { get { return _guiStyle; } }
        protected string Text { set { _text = value; } }
        protected Rect DisplayArea { set { _displayArea = value; } }
        protected Rect TextArea { set { _textArea = value; } }
        protected TextAnchor TextAlignment { set { _guiStyle.alignment = value; } }

        protected override void Start()
        {
            base.Start();

            _guiStyle = new GUIStyle();
            _guiStyle.alignment = TextAnchor.MiddleCenter;
            _guiStyle.font = Font;
            _guiStyle.fontSize = (int)(Font_Size * Scaling);
            _guiStyle.normal.textColor = Color.white;
            _guiStyle.wordWrap = true;
        }

        protected virtual void OnGUI()
        {
            GUI.DrawTexture(_displayArea, BackgroundTexture);
            GUI.Label(_textArea, _text, _guiStyle);
        }

        protected const float Margin = 10.0f;

        private const float Font_Size = 26.0f;
    }
}