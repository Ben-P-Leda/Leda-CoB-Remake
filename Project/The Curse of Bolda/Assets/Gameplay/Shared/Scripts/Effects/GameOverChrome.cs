using UnityEngine;

namespace Gameplay.Shared.Scripts.Effects
{
    public class GameOverChrome : MonoBehaviour
    {
        private void Awake()
        {
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
        }
    }
}
