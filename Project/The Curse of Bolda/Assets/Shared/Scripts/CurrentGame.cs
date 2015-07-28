using UnityEngine;

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

        public static void SetForNewLife()
        {
            _gameData.Energy = Constants.Player_Maximum_Energy;
        }

        public static void SetForLevelStart(int areaIndex, AreaStage stage, int requiredGems, Vector3 startPosition)
        {
            _gameData.Area = areaIndex;
            _gameData.Stage = stage;
            _gameData.GemsRequired = requiredGems;
            _gameData.GemsCollected = 0;
            _gameData.RestartPoint = startPosition;
            _gameData.CarryingKey = false;

            SetForNewLife();
        }

        private const int Starting_Lives = 5;
    }
}