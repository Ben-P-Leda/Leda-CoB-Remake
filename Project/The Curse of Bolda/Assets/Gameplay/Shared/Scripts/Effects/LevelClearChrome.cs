using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Effects
{
    public class LevelClearChrome : ScreenRelativeComponent
    {
        private bool _entryComplete;
        private GUIStyle _guiStyle;
        private Rect _boxRect;
        private int _timeBonus;
        private int _timeBonusDisplayCounter;
        private int _gemBonus;
        private int _gemBonusDisplayCounter;

        public Font Font;

        private void Awake()
        {
            _entryComplete = false;
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
        }

        protected override void Start()
        {
            base.Start();

            _guiStyle = new GUIStyle();
            _guiStyle.font = Font;
            _guiStyle.fontSize = (int)(Font_Size * Scaling);
            _guiStyle.normal.textColor = Color.white;
            _guiStyle.wordWrap = true;

            _boxRect = new Rect(Bonus_Box_Left * Scaling, Bonus_Box_Top * Scaling, Bonus_Box_Width * Scaling, Bonus_Box_Height * Scaling);
        }

        public void EntryCompleteCallback()
        {
            _entryComplete = true;
            _timeBonus = Time_Bonus_Increment * (int)Mathf.Floor(10.0f * (CurrentGame.GameData.TimeRemaining / CurrentGame.GameData.TotalTime));
            _timeBonusDisplayCounter = 0;
            _gemBonus = Gem_Bonus_Increment * (CurrentGame.GameData.GemsCollected - CurrentGame.GameData.GemsRequired);
            _gemBonusDisplayCounter = 0;
        }

        private void OnGUI()
        {
            if (_entryComplete)
            {
                _guiStyle.alignment = TextAnchor.UpperLeft;
                GUI.Label(_boxRect, "Time Bonus:\r\nGem Bonus:", _guiStyle);

                _guiStyle.alignment = TextAnchor.UpperRight;
                GUI.Label(_boxRect, string.Format("{0}\r\n{1}", _timeBonusDisplayCounter, _gemBonusDisplayCounter), _guiStyle);
            }
        }

        private void Update()
        {
            if (_entryComplete)
            {
                _timeBonusDisplayCounter = IncreaseBonusDisplayCounterAndScore(_timeBonusDisplayCounter, _timeBonus, Time_Bonus_Step);
                _gemBonusDisplayCounter = IncreaseBonusDisplayCounterAndScore(_gemBonusDisplayCounter, _gemBonus, Gem_Bonus_Step);
            }
        }

        private int IncreaseBonusDisplayCounterAndScore(int currentValue, int targetValue, int step)
        {
            if (currentValue < targetValue)
            {
                if (currentValue + step > targetValue) { step = targetValue - currentValue; }
                currentValue += step;
                CurrentGame.GameData.Score += step;
            }

            return currentValue;
        }

        private const float Font_Size = 26.0f;
        private const float Bonus_Box_Top = 390.0f;
        private const float Bonus_Box_Left = 455.0f;
        private const float Bonus_Box_Width = 290.0f;
        private const float Bonus_Box_Height = 90.0f;
        private const int Time_Bonus_Increment = 100;
        private const int Gem_Bonus_Increment = 20;
        private const int Time_Bonus_Step = 5;
        private const int Gem_Bonus_Step = 2;

    }
}
