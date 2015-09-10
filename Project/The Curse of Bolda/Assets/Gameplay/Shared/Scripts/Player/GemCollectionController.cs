using UnityEngine;

using Shared.Scripts;
using Gameplay.Shared.Scripts.Status_Display;
using Gameplay.Shared.Scripts.Effects;

namespace Gameplay.Shared.Scripts.Player
{
    public class GemCollectionController : MonoBehaviour
    {
        private GemCounter _gemCounter;

        public GameObject GemCountDisplay;

        private void Awake()
        {
            _gemCounter = GemCountDisplay.GetComponent<GemCounter>();
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if ((collider.tag == "Gem") && (CurrentGame.GameData.ActiveTool != ToolType.FireExtinguisher))
            {
                GemCollectSparklePool.ActivateGemCollectSparkle(collider.transform.position);

                collider.gameObject.SetActive(false);

                CurrentGame.GameData.GemsCollected++;
                _gemCounter.Refresh();
            }
        }
    }
}