using UnityEngine;

using Gameplay.Shared.Scripts.Player;
using Gameplay.Shared.Scripts.Shots;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class Type09Attack : MonoBehaviour
    {
        private Transform _transform;
        private EnemyLightningBolt _bolt;
        private Transform _playerTransform;

        public float DetectionRange;
        public GameObject LightningBolt;
        public GameObject Player;

        public bool Frozen { private get; set; }

        private void Awake()
        {
            _transform = transform;
            _playerTransform = Player.transform;
            _bolt = LightningBolt.GetComponent<EnemyLightningBolt>();
        }

        public void AttemptBoltLaunch()
        {
            if ((!LightningBolt.activeInHierarchy) && (!Frozen) && (Vector2.Distance(_transform.position, _playerTransform.position) <= DetectionRange))
            {
                Vector2 direction = _playerTransform.position - _transform.position;
                direction.Normalize();

                LightningBolt.SetActive(true);
                _bolt.Activate(_transform.position, direction);
            }
        }
    }
}