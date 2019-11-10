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

        public bool IsLocalUser => Id.Equals(UserData.Instance.User.Id);

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


    [Serializable]
    public struct UserMovement
    {
        public int UserId;
        public float Time;
        public Vector3 Position;
        public Quaternion Angle;
    }

    [Serializable]
    public struct UserRotation
    {
        public int UserId;
        public Quaternion Angle;
    }

    [Serializable]
    public struct UserShot
    {
        public int UserId;
        public Vector3 From;
        public Vector3 Direction;
    }

    [Serializable]
    public struct UserTakeDamage
    {
        public int UserId;
        public int Damage;
    }

    [Serializable]
    public struct UserPush
    {
        public int UserId;
        public int TargetId;
        public Vector3 Direction;
    }
}
