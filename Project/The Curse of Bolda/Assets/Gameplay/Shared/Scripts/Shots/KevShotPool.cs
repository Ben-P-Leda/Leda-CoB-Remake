using UnityEngine;
using Gameplay.Shared.Scripts.Generic;

namespace Gameplay.Shared.Scripts.Shots
{
    public class KevShotPool : GenericObjectPool<KevShot>
    {
        private Transform _playerTransform;

        public GameObject Player;

        protected override void Awake()
        {
            base.Awake();
            _playerTransform = Player.transform;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                KevShot shot = GetFirstAvailableObject();
                if (shot != null) 
                {
                    shot.Activate(_playerTransform.position, _playerTransform.localScale.x); 
                }
            }
        }

        protected override bool ObjectIsAvailable(KevShot objectToCheck)
        {
            return !objectToCheck.IsActive;
        }
    }
}