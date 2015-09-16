using UnityEngine;

namespace Gameplay.Boss.Scripts.Player
{
    public class ObstructionDetector : MonoBehaviour
    {
        private BossPlayerController _playerController;

        private void Awake()
        {
            _playerController = transform.parent.GetComponent<BossPlayerController>();
        }

        private void OnCollisionEnter2D(Collision2D collider)
        {
            _playerController.Obstructed = true;
        }

        private void OnCollisionExit2D(Collision2D collider)
        {
            _playerController.Obstructed = false;
        }
    }
}