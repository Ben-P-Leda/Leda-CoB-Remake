using UnityEngine;

namespace Shared.Scripts
{
    public class GameData
    {
        public int Lives { get; set; }
        public int Score { get; set; }
        public float Energy { get; set; }
        public int Area { get; set; }
        public AreaStage Stage { get; set; }
        public float TimeRemaining { get; set; }
        public int GemsRequired { get; set; }
        public int GemsCollected { get; set; }
        public Vector3 RestartPoint { get; set; }
        public Vector3 RestartScale { get; set; }
        public bool CarryingKey { get; set; }
        public int[] ToolCounts { get; set; }

        public GameData()
        {
            ToolCounts = new int[6];
        }
    }
}