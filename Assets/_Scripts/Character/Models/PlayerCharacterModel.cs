using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public class PlayerCharacterModel : CharacterModelBase
    {
        public PlayerCharacterModel(int id, float maxHealth, Vector3 position, string name = "default", IWeapon weapon = null) : base(id, maxHealth, position, name, weapon)
        {
        }
    }
}