using UnityEngine;

using Gameplay.Shared.Scripts.Player;
using Gameplay.Shared.Scripts.Shots;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class Type02Movement : Type01Movement
    {
        private EnemyMissile _missile;

        public float DetectionRange;
        public GameObject Missile;

        protected override void Start()
        {
            base.Start();

            _missile = Missile.GetComponent<EnemyMissile>();
        }

        protected override void Update()
        {
            base.Update();

            if (!Frozen)
            {
                Vector2 rayDirection = new Vector2(Mathf.Sign(HorizontalSpeed), 0.0f);
                RaycastHit2D ray = Physics2D.Raycast(Position, rayDirection, DetectionRange, PlayerSequencer.Player_Physics_Layer_Index+1);
                if ((ray.collider != null) && (ray.collider.tag == "Kev")) { AttemptMissileLaunch(); }
            }
        }

        private void AttemptMissileLaunch()
        {
            if (!Missile.activeInHierarchy)
            {
                Missile.SetActive(true);
                _missile.Activate(Position, Mathf.Sign(HorizontalSpeed));
            }
        }
    }
}