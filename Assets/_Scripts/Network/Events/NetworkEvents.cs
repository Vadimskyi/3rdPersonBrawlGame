using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public static class NetworkEvents
    {
        public static Action<string> onJoinGame;
        public static Action<User> onUserDataReceived;
        public static Action<string> onCharacterMoved;
        public static Action<string> onUserLeftChat;
        public static Action<string> onCharacterHit;
        public static Action<string> onCharacterShoot;

        internal static void JoinGame(string username)
        {
            onJoinGame?.Invoke(username);
        }

        internal static void UserDataReceived(User user)
        {
            onUserDataReceived?.Invoke(user);
        }
    }
}