using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Collectables
{
    public class EnergyRestore : CollectableBase
    {
        protected override void HandlePlayerContact()
        {
            CurrentGame.RestorePlayerEnergy();
            base.HandlePlayerContact();
        }
    }
}
