using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public class AxeCollisionDetection : MonoBehaviour
    {

        private void OnTriggerEnter(Collider collider)
        {
            var view = collider.gameObject.GetComponent<PlayerView>();
            if (view == null || !view.Facade.User.IsLocalUser) return;
            NetworkEvents.CharacterHitByTrap(TrapType.Axes);
        }
    }
}