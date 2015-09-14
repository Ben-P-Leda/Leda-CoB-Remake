using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Player
{
    public interface IPlayerSequencer
    {
        PlayerDeathSequence DeathSequence { set; }
    }
}
