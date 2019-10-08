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

        public DefaultGameRouter(SocketManager manager)
        {
            _manager = manager;

            /*NetworkEvents.onCharacterMoved += OnSendMessage;
            NetworkEvents.onCharacterHit += OnUserTyping;
            NetworkEvents.onCharacterShoot += OnUserStopTyping;*/
            NetworkEvents.onJoinGame += OnJoinGame;
            NetworkEvents.onCharacterMoved += OnMoveCharacter;
            NetworkEvents.onCharacterRotated += OnRotateCharacter;

            _manager.Socket.On("new_user_joined", NewUserJoined);
            _manager.Socket.On("user_data", OnGetUserData);
            _manager.Socket.On("combat_room_data", OnGetCombatRoomData);
            _manager.Socket.On("character_moved", OnCharacterMoved);
            _manager.Socket.On("character_rotated", OnCharacterRotated);

            _manager.Socket.On("user left", OnUserLeft);
            _manager.Socket.On("spawn character", OnSpawnCharacter);
            _manager.Socket.On("character shoot", OnCharacterShoot);
            _manager.Socket.On("character hit", OnCharacterHit);
            _manager.Socket.On("error", ErrorCallback);
        }

        private void OnJoinGame(string username)
        {
            _manager.Socket.Emit("join_game", username);
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
            Debug.Log("OnGetUserData");
            UserData.Instance.Initialize(JsonConvert.DeserializeObject<User>(args[0].ToString())); 
        }

        private void OnGetCombatRoomData(Socket socket, Packet packet, params object[] args)
        {
            Debug.Log("OnGetCombatRoomData");
            CombatRoomData.Instance.Initialize(JsonConvert.DeserializeObject<List<User>>(args[0].ToString()));
            //NetworkEvents.CombatRoomDataReceived();
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

        private void OnSpawnCharacter(Socket socket, Packet packet, params object[] args)
        {
            //NetworkEvents.NewMessageReceived(JsonConvert.DeserializeObject<ChatMessage>(args[0].ToString()));
        }

        private void OnCharacterShoot(Socket socket, Packet packet, params object[] args)
        {
            //NetworkEvents.UserStopTyping(args[0].ToString());
        }

        private void OnCharacterHit(Socket socket, Packet packet, params object[] args)
        {
            //Debug.Log("[Disconnect!]" + JsonConvert.SerializeObject(args));
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