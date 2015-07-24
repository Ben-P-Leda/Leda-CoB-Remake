using UnityEngine;

using Shared.Scripts;

namespace NonGame.Stage_Intro
{
    public class StageIntroPresenter : ScreenRelativeComponent
    {
        private GUIStyle _guiStyle;
        private Rect _displayArea;

        public Font Font;

        private void Awake()
        {
            _guiStyle = new GUIStyle();
            _guiStyle.alignment = TextAnchor.MiddleCenter;
            _guiStyle.font = Font;
            _guiStyle.fontSize = (int)(Font_Size * Scaling);
            _guiStyle.normal.textColor = Color.white;
            _guiStyle.wordWrap = true;

            _displayArea = new Rect(0, 0, Screen.width, Screen.height);
        }

        private void OnGUI()
        {
            GUI.Label(_displayArea, "Placeholder level intro screen\nPress any key...");
        }

        private void Update()
        {
            if (Input.anyKeyDown)
            {
                Application.LoadLevel("Level1-1");
            }
        }

        private const float Font_Size = 36.0f;
    }
}