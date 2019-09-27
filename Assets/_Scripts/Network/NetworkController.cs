using System;
using System.Collections;
using System.Collections.Generic;
using BestHTTP.SocketIO;
using UnityEngine;

namespace Vadimskyi.Game
{
    public class NetworkController : MonoBehaviour
    {
        private SocketManager _manager;
        private DefaultCombatRouter _router;

        private void Awake()
        { 
            SocketOptions options = new SocketOptions();
            options.AutoConnect = false;
            //options.QueryParamsOnlyForHandshake = false;
            options.Timeout = new TimeSpan(0, 0, 5, 0);
#if UNITY_EDITOR
            _manager = new SocketManager(new Uri("http://localhost:1111/socket.io/"), options);
#else
            _manager = new SocketManager(new Uri("http://95.179.129.130:1111/socket.io/"), options);
#endif
            _router = new DefaultCombatRouter(_manager);
            _manager.Open();
        }
    }
}