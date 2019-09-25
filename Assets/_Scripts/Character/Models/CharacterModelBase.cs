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
        public int Id { get; set; }
        public string Name { get; set; }
        public float MaxHealth { get; set; }
        public Vector3 Position { get; set; }

        [JsonIgnore]
        public IWeapon Weapon { get; set; }

        public ReactiveProperty<float> CurrentHealth { get; set; }

        protected CharacterModelBase(
            int id,
            float maxHealth,
            Vector3 position,
            string name = "default", 
            IWeapon weapon = null)
        {
            Id = id;
            Name = name;
            Weapon = weapon;
            Position = position;
            MaxHealth = maxHealth;
            CurrentHealth.Value = MaxHealth;
        }
    }
}