using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Collectables
{
    public class Tool : CollectableBase
    {
        public ToolType ToolType;

        protected override void HandlePlayerContact()
        {
            CurrentGame.GameData.ToolCounts[(int)ToolType]++;
            base.HandlePlayerContact();
        }
    }

    public enum ToolType
    {
        Invincibility = 0,
        Jetpack = 1,
        SuperJump = 2,
        FirepowerUp = 3,
        Pickaxe = 4,
        FireExtinguisher = 5
    }
}
