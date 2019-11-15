using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public static class NetworkEvents
    {
        public static event Action<string> onJoinGame = delegate { };
        public static event Action<TrapType> onCharacterHitByTrap = delegate { };
        public static event Action<User> onUserDataReceived = delegate { };
        public static event Action<User[]> onCombatRoomDataReceived = delegate { };

        public static event Action<UserMovement> onCharacterMoved = delegate { };
        public static event Action<UserRotation> onCharacterRotated = delegate { };
        public static event Action<UserShot> onCharacterFireGun = delegate { };
        public static event Action<UserTakeDamage> onCharacterTakeDamage = delegate { };
        public static event Action<UserPush> onCharacterPush = delegate { };
        public static event Action<int> onCharacterKick = delegate { };
        public static event Action<int> onCharacterDash = delegate { };
        public static event Action<int> onRespawnCharacter = delegate { };

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

        public static void CharacterFireGun(UserShot userShot)
        {
            onCharacterFireGun?.Invoke(userShot);
        }

        public static void CharacterTakeDamage(UserTakeDamage data)
        {
            onCharacterTakeDamage?.Invoke(data);
        }

        public static void RespawnCharacter(int userId)
        {
            onRespawnCharacter?.Invoke(userId);
        }

        public static void CharacterHitByTrap(TrapType trapType)
        {
            onCharacterHitByTrap?.Invoke(trapType);
        }

        public static void CharacterPush(UserPush userPush)
        {
            onCharacterPush?.Invoke(userPush);
        }

        public static void CharacterKick(int userId)
        {
            onCharacterKick?.Invoke(userId);
        }

        public static void CharacterDash(int userId)
        {
            onCharacterDash?.Invoke(userId);
        }
    }
}