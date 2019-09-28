using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public class CombatRoomData
    {
        public bool IsInitialized { get; set; }
        public List<User> Users => _users;

        private CombatRoomData()
        {
        }

        public void Initialize(List<User> users)
        {
            _users = users;
            IsInitialized = true;
        }

        private List<User> _users;
        public static CombatRoomData Instance => _instance ?? (_instance = new CombatRoomData());
        private static CombatRoomData _instance;
    }
}