using UnityEngine;

namespace Gameplay.Shared.Scripts.Collectables
{
    public class CollectableBase : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Kev") { HandlePlayerContact(); }
        }

        protected virtual void HandlePlayerContact() { }
    }
}