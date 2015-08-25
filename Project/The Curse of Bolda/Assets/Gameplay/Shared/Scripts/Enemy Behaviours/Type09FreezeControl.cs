using UnityEngine;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class Type09FreezeControl : MonoBehaviour, ICanBeFrozen
    {
        private Type09Attack _attackController = null;

        public bool Frozen
        {
            set
            {
                if (_attackController == null) { _attackController = transform.FindChild("Bolt").GetComponent<Type09Attack>(); }
                _attackController.Frozen = value;
            }
        }
    }
}