using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Collectables
{
    public class Key : CollectableBase
    {
        protected override void HandlePlayerContact()
        {
            if (!CurrentGame.GameData.CarryingKey)
            {
                CurrentGame.GameData.CarryingKey = true;
                base.HandlePlayerContact();
            }
        }
    }
}