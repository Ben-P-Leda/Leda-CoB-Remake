using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class BasicBehaviour : MonoBehaviour
    {
        protected GameObject GameObject { get; private set; }

        public int HitPoints;
        public int ScoreValue;
        public float PlayerEnergyDrainValue;

        protected virtual void Awake()
        {
            GameObject = transform.gameObject;
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

                GameObject.SetActive(false);
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
