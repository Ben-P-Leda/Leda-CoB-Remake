using UnityEngine;

namespace Gameplay.Shared.Scripts.Player
{
    public class SequencedPlayer : MonoBehaviour
    {
        public delegate void PlayerSequenceCompleteCallback();

        public PlayerSequenceCompleteCallback SequenceCompleteHandler { private get; set; }

        public void CompleteSequence()
        {
            if (SequenceCompleteHandler != null) { SequenceCompleteHandler(); }
        }
    }
}