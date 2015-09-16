using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Player
{
    public interface IPlayerSequencer
    {
        PlayerDeathSequence DeathSequence { set; }

        void StartNewLife();
        void StartDeathSequence(PlayerDeathSequence deathSequence, SequencedPlayer.PlayerSequenceCompleteCallback sequenceCompleteCallback);
    }
}
