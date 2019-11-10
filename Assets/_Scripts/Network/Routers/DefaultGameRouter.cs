using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP.SocketIO;
using Newtonsoft.Json;
using UnityEngine;
using Vadimskyi.Game;

namespace Vadimskyi.Game
{
    public class DefaultGameRouter
    {
        private SocketManager _manager;

        public DefaultGameRouter(SocketManager manager, NetworkSettings settings)
        {
            _manager = manager;

            NetworkEvents.onJoinGame += OnJoinGame;
            NetworkEvents.onCharacterMoved += OnMoveCharacter;
            NetworkEvents.onCharacterRotated += OnRotateCharacter;
            NetworkEvents.onCharacterFireGun += NetworkEvents_onCharacterFireGun;
            NetworkEvents.onCharacterTakeDamage += NetworkEvents_onCharacterTakeDamage;
            NetworkEvents.onRespawnCharacter += NetworkEvents_onRespawnCharacter;
            NetworkEvents.onCharacterHitByTrap += NetworkEvents_onCharacterHitByTrap;
            NetworkEvents.onCharacterPush += NetworkEvents_onCharacterPush;
            NetworkEvents.onCharacterKick += NetworkEvents_onCharacterKick;

            _manager.Socket.On("user_data", OnGetUserData);
            _manager.Socket.On("combat_room_data", OnGetCombatRoomData);
            _manager.Socket.On("character_moved", OnCharacterMoved);
            _manager.Socket.On("character_rotated", OnCharacterRotated);
            _manager.Socket.On("character_fired_gun", OnCharacterFiredGun);
            _manager.Socket.On("character_taken_damage", OnCharacterTakenDamage);
            _manager.Socket.On("character_respawned", OnCharacterRespawned);
            _manager.Socket.On("launch_traps", OnLaunchTraps);
            _manager.Socket.On("character_pushed", OnCharacterPushed);
            _manager.Socket.On("character_kick", OnCharacterKick);

            _manager.Socket.On("new_user_joined", NewUserJoined);
            _manager.Socket.On("user_left_game", OnUserLeft);

            _manager.Socket.On("error", ErrorCallback);
        }

        private void NetworkEvents_onCharacterKick(int userId)
        {
            _manager.Socket.Emit("character_kick", userId);
        }

        private void NetworkEvents_onCharacterPush(UserPush data)
        {
            _manager.Socket.Emit("push_character", data);
        }

        private void NetworkEvents_onCharacterHitByTrap(TrapType trapType)
        {
            _manager.Socket.Emit("character_hit_by_trap", new { UserId = UserData.Instance.User.Id, trapType = trapType.ToString()});
        }

        private void NetworkEvents_onRespawnCharacter(int userId)
        {
            _manager.Socket.Emit("character_respawn", new {userId});
        }

        private void OnJoinGame(string username)
        {
            _manager.Socket.Emit("join_game", username);
        }

        private void NetworkEvents_onCharacterFireGun(UserShot data)
        {
            _manager.Socket.Emit("fire_gun_character", data);
        }

        private void NetworkEvents_onCharacterTakeDamage(UserTakeDamage data)
        {
            _manager.Socket.Emit("take_damage_character", data);
        }

        private void OnMoveCharacter(UserMovement direction)
        {
            _manager.Socket.Emit("move_character", direction);
        }

        private void OnRotateCharacter(UserRotation angle)
        {
            _manager.Socket.Emit("rotate_character", angle);
        }

        private void OnGetUserData(Socket socket, Packet packet, params object[] args)
        {
            UserData.Instance.Initialize(JsonConvert.DeserializeObject<User>(args[0].ToString())); 
        }

        private void OnGetCombatRoomData(Socket socket, Packet packet, params object[] args)
        {
            CombatRoomData.Instance.Initialize(JsonConvert.DeserializeObject<List<User>>(args[0].ToString()));
        }

        private void NewUserJoined(Socket socket, Packet packet, params object[] args)
        {
            CombatRoomData.Instance.AddUser(JsonConvert.DeserializeObject<User>(args[0].ToString()));
        }

        private void OnCharacterMoved(Socket socket, Packet packet, params object[] args)
        {
            GameEvents.CharacterMoved(JsonConvert.DeserializeObject<UserMovement>(args[0].ToString()));
        }

        private void OnCharacterRotated(Socket socket, Packet packet, params object[] args)
        {
            GameEvents.CharacterRotated(JsonConvert.DeserializeObject<UserRotation>(args[0].ToString()));
        }

        private void OnCharacterFiredGun(Socket socket, Packet packet, params object[] args)
        {
            GameEvents.CharacterFiredGun(JsonConvert.DeserializeObject<UserShot>(args[0].ToString()));
        }

        private void OnCharacterTakenDamage(Socket socket, Packet packet, params object[] args)
        {
            GameEvents.CharacterTakenDamage(JsonConvert.DeserializeObject<UserTakeDamage>(args[0].ToString()));
        }

        private void OnCharacterRespawned(Socket socket, Packet packet, params object[] args)
        {
            GameEvents.CharacterRespawned(JsonConvert.DeserializeObject<PlayerCharacterModel>(args[0].ToString()));
        }

        private void OnCharacterPushed(Socket socket, Packet packet, params object[] args)
        {
            GameEvents.CharacterPushed(JsonConvert.DeserializeObject<UserPush>(args[0].ToString()));
        }

        private void OnCharacterKick(Socket socket, Packet packet, params object[] args)
        {
            GameEvents.CharacterKick(JsonConvert.DeserializeObject<int>(args[0].ToString()));
        }

        private void OnLaunchTraps(Socket socket, Packet packet, params object[] args)
        {
            Enum.TryParse(args[0].ToString(), out TrapType trapType);
            GameEvents.LaunchSpikeTraps(trapType);
        }

        private void OnUserLeft(Socket socket, Packet packet, params object[] args)
        {

        }

        private void ErrorCallback(Socket socket, Packet packet, params object[] args)
        {
            Debug.Log("[Error!]" + JsonConvert.SerializeObject(args));
        }
    }
}