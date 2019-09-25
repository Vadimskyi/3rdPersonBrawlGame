using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public interface IWeapon
    {
        float Damage { get; }
        WeaponType WeaponType { get; }
    }

    public enum WeaponType
    {
        Default,
        RocketLauncher
    }
}