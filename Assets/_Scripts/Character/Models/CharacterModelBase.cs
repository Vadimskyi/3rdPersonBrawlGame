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
        public float Damage { get; set; }
        public int MaxHealth { get; set; }
        public bool IsDead => CurrentHealth.Value <= 0;
        public ReactiveProperty<Vector3> Position { get; set; }
        public Quaternion Rotation { get; set; }

        [JsonIgnore]
        public IWeapon Weapon { get; set; }

        public ReactiveProperty<int> CurrentHealth { get; set; }
        public ReactiveCommand<CharacterModelBase> OnReset;

        protected CharacterModelBase(
            int maxHealth,
            Vector3 position,
            IWeapon weapon = null)
        {
            Weapon = weapon;
            MaxHealth = maxHealth;
            Position = new ReactiveProperty<Vector3>(position);
            CurrentHealth = new ReactiveProperty<int>(MaxHealth);
            OnReset = new ReactiveCommand<CharacterModelBase>();
        }
    }
}