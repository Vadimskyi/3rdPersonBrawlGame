using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public class CombatRoomData
    {
        public event Action<User> OnNewUserJoined = delegate {  }; 
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

        public void AddUser(User user)
        {
            _users?.Add(user);
            OnNewUserJoined?.Invoke(user);
        }

        private List<User> _users;
        public static CombatRoomData Instance => _instance ?? (_instance = new CombatRoomData());
        private static CombatRoomData _instance;
    }
}