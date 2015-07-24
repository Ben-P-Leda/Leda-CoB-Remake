using UnityEngine;

namespace Gameplay.Shared.Scripts.Status_Display
{
    public class DisplayBase : MonoBehaviour
    {
        private Rect _displayArea;
        private Rect _textArea;
        private GUIStyle _guiStyle;
        private string _text;
        private float _scaling;

        public Texture2D BackgroundTexture;
        public Font Font;

        protected float Scaling { get { return _scaling; } }
        protected GUIStyle GuiStyle { get { return _guiStyle; } }
        protected string Text { set { _text = value; } }
        protected Rect DisplayArea { set { _displayArea = value; } }
        protected Rect TextArea { set { _textArea = value; } }
        protected TextAnchor TextAlignment { set { _guiStyle.alignment = value; } }

        protected virtual void Start()
        {
            _scaling = Screen.height / One_To_One_Screen_Height;

            _guiStyle = new GUIStyle();
            _guiStyle.alignment = TextAnchor.MiddleCenter;
            _guiStyle.font = Font;
            _guiStyle.fontSize = (int)(Font_Size * _scaling);
            _guiStyle.normal.textColor = Color.white;
            _guiStyle.wordWrap = true;
        }

        protected virtual void OnGUI()
        {
            GUI.DrawTexture(_displayArea, BackgroundTexture);
            GUI.Label(_textArea, _text, _guiStyle);
        }

        protected const float Margin = 10.0f;

        private const float One_To_One_Screen_Height = 675.0f;
        private const float Font_Size = 26.0f;
    }
}