using UnityEngine;

namespace Gameplay.Boss.Scripts.Boss_Behaviour
{
    public class BossFrontCollider : MonoBehaviour
    {
        public bool IsInCollision { get; private set; }

        private void Reset()
        {
            IsInCollision = false;
        }

        private void OnCollisionEnter2D(Collision2D collider)
        {
            if (collider.gameObject.tag == "Arena Boundary") { IsInCollision = true; }
        }

        private void OnCollisionExit2D(Collision2D collider)
        {
            Reset();
        }
    }
}