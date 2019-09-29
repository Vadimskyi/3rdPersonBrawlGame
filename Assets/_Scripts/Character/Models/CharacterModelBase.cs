using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Vadimskyi.Game
{
    [Serializable]
    public abstract class CharacterModelBase
    {
        public int UserId { get; set; }
        public float MaxHealth { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        [JsonIgnore]
        public IWeapon Weapon { get; set; }

        public ReactiveProperty<float> CurrentHealth { get; set; }

        protected CharacterModelBase(
            float maxHealth,
            Vector3 position,
            IWeapon weapon = null)
        {
            Weapon = weapon;
            Position = position;
            MaxHealth = maxHealth;
            CurrentHealth = new ReactiveProperty<float>(MaxHealth);
        }
    }
}