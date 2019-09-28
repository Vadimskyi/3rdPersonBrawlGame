using Vadimskyi.Utils;

namespace Vadimskyi.Game
{
    public class GameStateController : StateController
    {
        private GameStateFactory _gameStateFactory;

        public GameStateController(GameStateFactory gameStateFactory) : base()
        {
            _instance = this;
            _gameStateFactory = gameStateFactory;
        }

        public virtual void SetState(GameStateType stateType, object args = null)
        {
            base.SetState(_gameStateFactory.Create(stateType), args);
        }

        public static GameStateController Instance => _instance;
        private static GameStateController _instance;
    }
}
