using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public static class GameEvents
    {
        public static event Action<string> onUserEnterPlayMode = delegate { };

        public static void UserEnterPlayMode(string username)
        {
            onUserEnterPlayMode?.Invoke(username);
        }
    }
}
