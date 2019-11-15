using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public static class GameEvents
    {
        public static event Action<TrapType> onLaunchSpikeTraps = delegate { };
        public static event Action<UserMovement> onCharacterMoved = delegate { };
        public static event Action<UserRotation> onCharacterRotated = delegate { };
        public static event Action<UserShot> onCharacterFiredGun = delegate { };
        public static event Action<UserTakeDamage> onCharacterTakenDamage = delegate { };
        public static event Action<UserPush> onCharacterPushed = delegate { };
        public static event Action<int> onCharacterKick = delegate { };
        public static event Action<int> onCharacterDash = delegate { };
        public static event Action<PlayerCharacterModel> onCharacterRespawned = delegate { };

        internal static void CharacterFiredGun(UserShot data)
        {
            onCharacterFiredGun?.Invoke(data);
        }

        internal static void CharacterMoved(UserMovement direction)
        {
            onCharacterMoved?.Invoke(direction);
        }

        internal static void CharacterRotated(UserRotation angle)
        {
            onCharacterRotated?.Invoke(angle);
        }

        public static void CharacterTakenDamage(UserTakeDamage data)
        {
            onCharacterTakenDamage?.Invoke(data);
        }

        public static void CharacterRespawned(PlayerCharacterModel data)
        {
            onCharacterRespawned?.Invoke(data);
        }

        public static void LaunchSpikeTraps(TrapType type)
        {
            onLaunchSpikeTraps?.Invoke(type);
        }

        public static void CharacterPushed(UserPush data)
        {
            onCharacterPushed.Invoke(data);
        }

        public static void CharacterKick(int userId)
        {
            onCharacterKick.Invoke(userId);
        }

        public static void CharacterDash(int userId)
        {
            onCharacterDash.Invoke(userId);
        }
    }
}
