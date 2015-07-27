using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Status_Display
{
    public class PlayerStatus : DisplayBase
    {
        private Rect _energyBarContainer;
        private float _displayedEnergy;

        public Texture2D EnergyBarTexture;

        protected override void Start()
        {
            base.Start();

            DisplayArea = new Rect(Margin * Scaling, Margin * Scaling, BackgroundTexture.width * Scaling, BackgroundTexture.height * Scaling);
            TextArea = new Rect(
                (Margin + Text_Horizontal_Offset) * Scaling, 
                (Margin + Text_Vertical_Offset) * Scaling,
                Text_Area * Scaling,
                Text_Area * Scaling);

            _displayedEnergy = 0.0f;
        }

        private void Update()
        {
            if (_displayedEnergy != CurrentGame.GameData.Energy)
            {
                if (Mathf.Abs(CurrentGame.GameData.Energy - _displayedEnergy) < Energy_Bar_Step) { _displayedEnergy = CurrentGame.GameData.Energy; }
                else { _displayedEnergy += (Mathf.Sign(CurrentGame.GameData.Energy - _displayedEnergy) * Energy_Bar_Step); }

                _energyBarContainer = new Rect(
                    (Margin + Energy_Bar_Horizontal_Offset) * Scaling,
                    (Margin + Energy_Bar_Vertical_Offset) * Scaling,
                    Mathf.Max((Energy_Bar_Max_Width * (_displayedEnergy / Constants.Player_Maximum_Energy)) * Scaling, 0.0f),
                    EnergyBarTexture.height * Scaling);
            }

            Text = CurrentGame.GameData.Lives.ToString();
        }

        protected override void OnGUI()
        {
            base.OnGUI();

            GUI.DrawTexture(_energyBarContainer, EnergyBarTexture);
        }

        private const float Text_Horizontal_Offset = 65.0f;
        private const float Text_Vertical_Offset = 30.0f;
        private const float Text_Area = 30.0f;
        private const float Energy_Bar_Horizontal_Offset = 71.0f;
        private const float Energy_Bar_Vertical_Offset = 13.0f;
        private const float Energy_Bar_Max_Width = 100.0f;
        private const float Energy_Bar_Step = 5.0f;
    }
}