using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts
{
    public class LevelCoordinator : MonoBehaviour
    {
        private static LevelCoordinator _instance = null;

        private static void UpdateGameData(DataItem itemToUpdate, int delta)
        {
            switch (itemToUpdate)
            {
                case DataItem.Gems: _instance.UpdateGemCount(delta); break;
                case DataItem.Score: _instance.UpdateScore(delta); break;
                case DataItem.Energy: _instance.UpdateEnergy(delta); break;
            }
        }

        private void Awake()
        {
            if (_instance == null) { _instance = this; }
        }

        private void Update()
        {
            // TODO: This should update CurrentGame.GameData.TimeRemaining and display
        }

        private void UpdateGemCount(int delta)
        {

        }

        private void UpdateScore(int delta)
        {

        }

        private void UpdateEnergy(int delta)
        {

        }
    }
}
