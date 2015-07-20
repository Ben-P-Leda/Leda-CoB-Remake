using UnityEngine;

namespace Gameplay.Shared.Scripts.Status_Display
{
    public class GemCounter : MonoBehaviour
    {
        private Rect _displayArea;
        private Rect _textArea;
        private GUIStyle _guiStyle;
        private float _fontSize;

        public Texture2D BackgroundTexture;
        public Font Font;

        private void Start()
        {
            float scaling = Screen.height / One_To_One_Screen_Height;

            _displayArea = new Rect(
                Screen.width - ((BackgroundTexture.width + Margin) * scaling),
                Screen.height - ((BackgroundTexture.height + Margin) * scaling),
                BackgroundTexture.width * scaling,
                BackgroundTexture.height * scaling);

            _textArea = new Rect(_displayArea.x, _displayArea.y, _displayArea.width - _displayArea.height, _displayArea.height);

            _guiStyle = new GUIStyle();
            _guiStyle.alignment = TextAnchor.MiddleCenter;
            _guiStyle.font = Font;
            _guiStyle.fontSize = (int)(Font_Size * scaling);
            _guiStyle.normal.textColor = Color.white;
        }

        private void OnGUI()
        {
            GUI.DrawTexture(_displayArea, BackgroundTexture);
            GUI.Label(_textArea, "0", _guiStyle);
        }

        private const float Margin = 10.0f;
        private const float One_To_One_Screen_Height = 675.0f;
        private const float Font_Size = 26.0f;
    }
}