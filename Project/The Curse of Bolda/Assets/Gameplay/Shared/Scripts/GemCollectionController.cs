using UnityEngine;

namespace Gameplay.Shared.Scripts
{
    public class GemCollectionController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Gem")
            {
                // TODO: All effects and status updates

                collider.gameObject.SetActive(false);
            }
        }
    }
}