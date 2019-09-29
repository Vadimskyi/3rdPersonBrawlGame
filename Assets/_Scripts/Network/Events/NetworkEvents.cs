using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public static class NetworkEvents
    {
        public static event Action<string> onJoinGame = delegate { };
        public static event Action<User> onUserDataReceived = delegate { };
        public static event Action<User[]> onCombatRoomDataReceived = delegate { };

        public static event Action<UserMovement> onCharacterMoved = delegate { };
        public static event Action<UserRotation> onCharacterRotated = delegate { };
        public static Action<string> onUserLeftChat;
        public static Action<string> onCharacterHit;
        public static Action<string> onCharacterShoot;

        internal static void JoinGame(string username)
        {
            onJoinGame?.Invoke(username);
        }

        internal static void CharacterMoved(UserMovement direction)
        {
            onCharacterMoved?.Invoke(direction);
        }

        internal static void CharacterRotated(UserRotation angle)
        {
            onCharacterRotated?.Invoke(angle);
        }

        internal static void UserDataReceived(User user)
        {
            onUserDataReceived?.Invoke(user);
        }

        public static void CombatRoomDataReceived(User[] users)
        {
            onCombatRoomDataReceived?.Invoke(users);
        }
    }
}