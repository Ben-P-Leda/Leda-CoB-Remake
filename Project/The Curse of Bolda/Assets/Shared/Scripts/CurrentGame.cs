﻿using UnityEngine;

namespace Shared.Scripts
{
    public static class CurrentGame
    {
        private static GameData _gameData = new GameData();
        public static GameData GameData { get { return _gameData; } }

        public static void SetForNewGame()
        {
            _gameData.Lives = Starting_Lives; ;
            _gameData.Score = 0;
        }

        public static void RestorePlayerEnergy()
        {
            _gameData.Energy = Constants.Player_Maximum_Energy;
        }

        public static void SetForLevelStart(int areaIndex, AreaStage stage, int requiredGems, Vector3 startPosition, bool facingLeft)
        {
            _gameData.Area = areaIndex;
            _gameData.Stage = stage;
            _gameData.GemsRequired = requiredGems;
            _gameData.GemsCollected = 0;
            _gameData.RestartPoint = startPosition;
            _gameData.RestartScale = new Vector3(facingLeft ? -1.0f : 1.0f, 1.0f, 1.0f);
            _gameData.CarryingKey = false;

            if (stage == AreaStage.One) { ClearInventory(); }

            RestorePlayerEnergy();
        }

        private static void ClearInventory()
        {
            for (int i = 0; i < _gameData.ToolCounts.Length; i++) { _gameData.ToolCounts[i] = 0; }
        }

        public static void AddTool(ToolType toolType)
        {
            _gameData.ToolCounts[(int)toolType] += 1;
        }

        public static bool HasTool(ToolType toolType)
        {
            return _gameData.ToolCounts[(int)toolType] > 0;
        }

        public static void ActivateTool(ToolType toolType)
        {
            _gameData.ToolCounts[(int)toolType] -= 1;

            switch (toolType)
            {
                case ToolType.FireExtinguisher: _gameData.ToolActiveTimeRemaining = Constants.Fire_Extinguisher_Duration; break;
            }
        }

        public static bool ToolIsActive { get { return _gameData.ToolActiveTimeRemaining > 0.0f; } }

        private const int Starting_Lives = 5;
    }
}