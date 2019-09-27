using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public class PlayerCharacterModel : CharacterModelBase
    {
        public PlayerCharacterModel(float maxHealth, Vector3 position, IWeapon weapon = null) : base(maxHealth, position, weapon)
        {
        }
    }
}