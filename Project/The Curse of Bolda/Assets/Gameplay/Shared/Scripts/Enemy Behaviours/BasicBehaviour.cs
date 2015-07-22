using UnityEngine;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class BasicBehaviour : MonoBehaviour
    {
        private GameObject _gameObject;

        public int HitPoints;
        public int ScoreValue;
        public bool PlayerShotsDoNoDamage;

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
                    //  - Award score

                    _gameObject.SetActive(false);
                }
            }
        }
    }
}
