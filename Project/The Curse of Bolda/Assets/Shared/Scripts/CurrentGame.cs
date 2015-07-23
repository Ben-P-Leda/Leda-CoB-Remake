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
            _gameData.Energy = Maximum_Energy;
        }

        public static void SetForLevelStart(int areaIndex, AreaStage stage, float durationInSeconds, int requiredGems)
        {
            _gameData.Area = areaIndex;
            _gameData.Stage = stage;
            _gameData.TimeRemaining = durationInSeconds;
            _gameData.GemsRequired = requiredGems;
            _gameData.GemsCollected = 0;

            SetForNewLife();
        }

        private const int Starting_Lives = 5;
        private const float Maximum_Energy = 100.0f;
    }
}