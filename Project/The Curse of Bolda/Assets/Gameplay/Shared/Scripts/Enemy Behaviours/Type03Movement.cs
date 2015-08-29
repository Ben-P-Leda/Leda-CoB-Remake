using UnityEngine;

using Gameplay.Shared.Scripts.Player;
using Gameplay.Shared.Scripts.Shots;

namespace Gameplay.Shared.Scripts.Enemy_Behaviours
{
    public class Type03Movement : Type01Movement
    {
        protected override void HandleCollisionWithPlayerShot(int hitPointDelta)
        {
            // Enemy is invulerable, do nothing, maybe trigger an effect...
        }
    }
}