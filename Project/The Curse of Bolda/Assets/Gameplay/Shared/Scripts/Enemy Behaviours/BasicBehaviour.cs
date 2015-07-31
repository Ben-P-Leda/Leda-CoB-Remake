using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class BasicBehaviour : MonoBehaviour
    {
        private GameObject _gameObject;

        public int HitPoints;
        public int ScoreValue;
        public float PlayerEnergyDrainValue;

        protected virtual void Awake()
        {
            _gameObject = transform.gameObject;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Kev Shot") 
            { 
                HandleCollisionWithPlayerShot(1);
                collider.enabled = false;
            }
        }

        protected virtual void HandleCollisionWithPlayerShot(int hitPointDelta)
        {
            HitPoints -= hitPointDelta;
            if (HitPoints < 1)
            {
                //  - Start death effect
                CurrentGame.GameData.Score += ScoreValue;

                _gameObject.SetActive(false);
            }
        }

        protected virtual void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.tag == "Kev") { HandleCollisionWithPlayer(); }
        }

        protected virtual void HandleCollisionWithPlayer()
        {
            CurrentGame.GameData.Energy -= PlayerEnergyDrainValue;
        }
    }
}
