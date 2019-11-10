using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vadimskyi.Utils;

namespace Vadimskyi.Game
{
    public class BattleArenaController : MonoBehaviour
    {
        private List<TrapAnimation> _trapList;
        private void Awake()
        {
            _trapList = GetComponentsInChildren<TrapAnimation>().ToList();
            GameEvents.onLaunchSpikeTraps += GameEvents_onLaunchSpikeTraps;
        }

        private void GameEvents_onLaunchSpikeTraps(TrapType type)
        {
            _trapList.Where(s => s.TrapType.Equals(type)).ForEach(t => t.LaunchTrap());
        }
    }
}
