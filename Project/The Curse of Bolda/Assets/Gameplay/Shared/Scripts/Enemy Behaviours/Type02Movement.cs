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
        }
    }
}