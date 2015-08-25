using UnityEngine;

using Shared.Scripts;
using Gameplay.Shared.Scripts.Effects;

namespace Gameplay.Shared.Scripts.Shots
{
    public class EnemyLightningBolt : MonoBehaviour
    {
        private Transform _transform = null;
        private Rigidbody2D _rigidBody2D = null;

        public void Activate(Vector2 position, Vector2 direction)
        {
            if (_transform == null) { _transform = transform; }
            if (_rigidBody2D == null) { _rigidBody2D = GetComponent<Rigidbody2D>(); }

            _transform.position = position;
            _rigidBody2D.velocity = direction * Movement_Speed;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Deactivate();
        }

        private void Deactivate()
        {
            BoltBurstPool.ActivateBoltBurst(_transform.position);
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Kev")
            {
                CurrentGame.GameData.Energy -= Energy_Drain_Value;
                Deactivate();
            }
        }

        private const float Movement_Speed = 5.0f;
        private const float Energy_Drain_Value = 80.0f; 
    }
}