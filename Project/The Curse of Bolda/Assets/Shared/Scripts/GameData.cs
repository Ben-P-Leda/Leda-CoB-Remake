using UnityEngine;

namespace Shared.Scripts
{
    public class GameData
    {
        public int Lives { get; set; }
        public int Score { get; set; }
        public float Energy { get; set; }
        public float Area { get; set; }
        public AreaStage Stage { get; set; }
        public float TimeRemaining { get; set; }
        public int GemsRequired { get; set; }
        public int GemsCollected { get; set; }
        public Vector3 RestartPoint { get; set; }
    }
}