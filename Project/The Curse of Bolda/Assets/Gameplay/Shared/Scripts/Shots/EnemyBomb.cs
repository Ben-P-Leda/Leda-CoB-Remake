using UnityEngine;

using Shared.Scripts;
using Gameplay.Shared.Scripts.Effects;

namespace Gameplay.Shared.Scripts.Shots
{
    public class EnemyBomb : MonoBehaviour
    {
        private Transform _transform;
        private Rigidbody2D _rigidBody2D;
        private GameObject _gameObject;

        private void Awake()
        {
            _transform = transform;
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _gameObject = gameObject;
        }

        public void Activate(Vector2 position)
        {
            _transform.position = position;
            _rigidBody2D.velocity = new Vector2(0.0f, Initial_Drop_Velocity);
        }

        private void Update()
        {
            if (_rigidBody2D.velocity.y > Initial_Drop_Velocity) { Deactivate(); }
        }

        private void Deactivate()
        {
            FireballPool.ActivateFireball(_transform.position);
            _gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Kev")
            {
                CurrentGame.GameData.Energy -= Energy_Drain_Value;
                Deactivate();
            }
        }

        private const float Initial_Drop_Velocity = -0.05f;
        private const float Energy_Drain_Value = 80.0f; 
    }
}