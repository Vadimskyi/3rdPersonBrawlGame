using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    [Serializable]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PlayerCharacterModel Character;

        public User(int id, string name, PlayerCharacterModel character)
        {
            Id = id;
            Name = name;
            Character = character;
        }

        public User()
        {
        }
    }
}
