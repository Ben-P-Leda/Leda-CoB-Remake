using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Collectables
{
    public class Tool : CollectableBase
    {
        public ToolType ToolType;

        protected override void HandlePlayerContact()
        {
            CurrentGame.AddTool(ToolType);
            base.HandlePlayerContact();
        }
    }
}
