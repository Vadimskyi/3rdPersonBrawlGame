using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public class PlayerFacade : MonoBehaviour
    {
        public PlayerView View => _view;
        private PlayerView _view;

        private void Awake()
        {
            _view = GetComponent<PlayerView>();
        }

        public void Initialize()
        {

        }
    }
}
