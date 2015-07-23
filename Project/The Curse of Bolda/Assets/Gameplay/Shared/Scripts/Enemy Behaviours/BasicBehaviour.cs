using UnityEngine;

using Shared.Scripts;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class BasicBehaviour : MonoBehaviour
    {
        private GameObject _gameObject;

        public int HitPoints;
        public int ScoreValue;
        public bool PlayerShotsDoNoDamage;
        public float PlayerEnergyDrainValue;

        protected virtual void Awake()
        {
            _gameObject = transform.gameObject;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if ((collider.tag.StartsWith("Kev Shot")) &&  (!PlayerShotsDoNoDamage))
            {
                HitPoints -= 1;
                if (HitPoints < 1)
                {
                    //  - Start death effect
                    CurrentGame.GameData.Score += ScoreValue;

                    _gameObject.SetActive(false);
                }
            }
        }
    }
}
