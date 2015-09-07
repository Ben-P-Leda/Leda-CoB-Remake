using UnityEngine;

using Shared.Scripts;
using Gameplay.Shared.Scripts.Effects;

namespace Gameplay.Shared.Scripts.Shots
{
    public class EnemyMissile : MonoBehaviour
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

        public void Activate(Vector2 position, float direction)
        {
            _transform.position = position;
            _transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f * direction);
            _rigidBody2D.velocity = new Vector2(Movement_Speed * direction, 0.0f);
        }

        private void Update()
        {
            if (Mathf.Abs(_rigidBody2D.velocity.x) < Movement_Speed) { Deactivate(); }
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
                if (CurrentGame.GameData.ActiveTool != ToolType.Invincibility)
                {
                    CurrentGame.GameData.Energy -= Energy_Drain_Value;
                }
                Deactivate();
            }
        }

        private const float Movement_Speed = 4.0f;
        private const float Energy_Drain_Value = 30.0f; 
    }
}