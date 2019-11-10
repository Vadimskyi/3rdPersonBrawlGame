using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public class PlayerCharacterModel : CharacterModelBase
    {
        public PlayerCharacterModel(int maxHealth, Vector3 position, IWeapon weapon = null) : base(maxHealth, position, weapon)
        {
        }

        public void ResetToDefaults(PlayerCharacterModel data)
        {
            MaxHealth = data.MaxHealth;
            CurrentHealth.SetValueAndForceNotify(data.MaxHealth);
            Damage = data.Damage;
            Position.SetValueAndForceNotify(data.Position.Value);
            Rotation = data.Rotation;
            OnReset?.Execute(this);
        }
    }
}