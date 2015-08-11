using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class BasicBehaviour : MonoBehaviour
    {
        private float _lastHitTime;

        protected GameObject GameObject { get; private set; }

        public int HitPoints;
        public int ScoreValue;
        public float PlayerEnergyDrainValue;

        protected virtual void Awake()
        {
            GameObject = transform.gameObject;

            _lastHitTime = 0.0f;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Kev Shot")
            {
                // Nasty little hack to make sure that we only take the right number of hits
                if (Time.timeSinceLevelLoad - _lastHitTime > Minimum_Time_Between_Hits) { HandleCollisionWithPlayerShot(1); }
                _lastHitTime = Time.timeSinceLevelLoad;

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

        private float Minimum_Time_Between_Hits = 0.0001f;
    }
}
