using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Collectables
{
    public class EnergyRestore : BouncingCollectableBase
    {
        protected override void HandlePlayerContact()
        {
            CurrentGame.RestorePlayerEnergy();
            base.HandlePlayerContact();
        }
    }
}
