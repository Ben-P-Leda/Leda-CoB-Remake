using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Collectables
{
    public class Tool : BouncingCollectableBase
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
        Invincibility = 1,
        Jetpack = 2,
        SuperJump = 3,
        FirepowerUp = 4,
        Pickaxe = 5,
        FireExtinguisher = 6
    }
}
