using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public static class GameEvents
    {
        public static event Action<UserMovement> onCharacterMoved = delegate { };
        public static event Action<UserRotation> onCharacterRotated = delegate { };

        internal static void CharacterMoved(UserMovement direction)
        {
            onCharacterMoved?.Invoke(direction);
        }

        internal static void CharacterRotated(UserRotation angle)
        {
            onCharacterRotated?.Invoke(angle);
        }
    }
}
