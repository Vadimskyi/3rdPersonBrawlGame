using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vadimskyi.Utils;

namespace Vadimskyi.Game
{
    public class GameStateFactory : IFactory<GameState, GameStateType>
    {
        public GameStateFactory()
        {

        }

        public GameState Create(GameStateType type)
        {
            switch (type)
            {
                case GameStateType.Enter:
                    return new GameEnterState();
                case GameStateType.Networking:
                    return new GameNetworkingState();
                case GameStateType.LoadArena:
                    return new GameLoadArenaState();
                case GameStateType.Battle:
                    return new GameBattleState();
            }

            throw new NotImplementedException($"{type} Game State Type is not implemented!");
        }

        public void Dispose()
        {
        }
    }
}
