using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Player
{
    public class PlayerSequencer : MonoBehaviour
    {
        public GameObject Player;

        public int Area;
        public AreaStage Stage;
        public float DurationInSeconds;
        public int RequiredGems;

        private void Awake()
        {

        }

        private void OnLevelWasLoaded()
        {
            CurrentGame.SetForLevelStart(Area, Stage, DurationInSeconds, RequiredGems);
        }
    }
}