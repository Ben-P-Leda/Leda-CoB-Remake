using UnityEngine;

using Shared.Scripts;

using Gameplay.Normal.Scripts.Status_Display;

namespace Gameplay.Normal.Scripts.Activators
{
    public class Lock : ActivatorBase
    {
        private bool _isUnlocked;

        protected override void Awake()
        {
            base.Awake();

            _isUnlocked = false;
        }

        protected override void SetActive(bool isActive)
        {
            if ((isActive) && (!_isUnlocked) && (CurrentGame.GameData.CarryingKey))
            {
                base.SetActive(isActive);
                CurrentGame.GameData.CarryingKey = false;
                _isUnlocked = true;
            }
        }
    }
}