using Vadimskyi.Utils;

namespace Vadimskyi.Game
{
    public abstract class GameState : IState
    {
        public GameStateType GameStateType {get;}

        protected GameState(GameStateType gameStateType)
        {
            GameStateType = gameStateType;
        }

        public abstract void OnUpdate();
        public abstract void OnStateEnter(object args = null);
        public abstract void OnStateExit();

        public void Dispose()
        {

        }
    }

    public enum GameStateType
    {
        Default,
        Enter,
        Networking,
        LoadArena,
        Battle
    }
}
