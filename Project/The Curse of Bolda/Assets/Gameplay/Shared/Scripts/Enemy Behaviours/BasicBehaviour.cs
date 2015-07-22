using UnityEngine;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class BasicBehaviour : MonoBehaviour
    {
        public int HitPoints;
        public int ScoreValue;
        public bool PlayerShotsDoNoDamage;

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.tag == "Player Shot")
            {
                if (!PlayerShotsDoNoDamage)
                {
                    // TODO:
                    //      - Get player shot script
                    //      - Subtract damage value from hit points

                    if (HitPoints < 1)
                    {
                        //  - Switch off enemy
                        //  - Start death effect
                        //  - Award score
                    }

                    //      - Switch off player shot
                }
            }
        }
    }
}
