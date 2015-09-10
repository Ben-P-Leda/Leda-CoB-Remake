using UnityEngine;

using Gameplay.Normal.Scripts.Player_Control;
using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Player
{
    public class Pickaxe : MonoBehaviour
    {
        private KevActionController _playerController;

        private void Awake()
        {
            _playerController = transform.parent.GetComponent<KevActionController>();
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