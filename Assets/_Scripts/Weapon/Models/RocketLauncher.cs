using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public class RocketLauncher : IWeapon
    {
        public float Damage { get; private set; }
        public WeaponType WeaponType { get; private set; }

        public RocketLauncher(float damage, WeaponType weaponType)
        {
            Damage = damage;
            WeaponType = weaponType;
        }
    }
}