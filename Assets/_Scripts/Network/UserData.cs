using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public class UserData
    {
        public bool IsInitialized { get; set; }
        public User User => _user;

        private UserData()
        {
        }

        public void Initialize(User user)
        {
            _user = user;
            IsInitialized = true;
        }

        private User _user;
        public static UserData Instance => _instance ?? (_instance = new UserData());
        private static UserData _instance;
    }
}
