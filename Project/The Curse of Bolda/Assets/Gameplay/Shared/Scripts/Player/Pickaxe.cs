using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Player
{
    public class Pickaxe : MonoBehaviour
    {
        private InputDrivenPlayer _playerController;

        private void Awake()
        {
            _playerController = transform.parent.GetComponent<InputDrivenPlayer>();
        }

        private void OnCollisionEnter2D(Collision2D collider)
        {
            if (CurrentGame.GameData.ActiveTool == ToolType.Pickaxe)
            {
                _playerController.PickaxeHasGripped = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collider)
        {
            _playerController.PickaxeHasGripped = false;
        }
    }
}