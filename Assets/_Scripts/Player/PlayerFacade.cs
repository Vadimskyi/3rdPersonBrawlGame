using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public class PlayerFacade
    {
        public PlayerView View => _view;
        public PlayerHealth Health => _health;
        public PlayerMovement Movement => _movement;
        public PlayerShooting Shooting => _shooting;

        private User _model;
        private PlayerView _view;
        private PlayerHealth _health;
        private PlayerMovement _movement;
        private PlayerShooting _shooting;

        public PlayerFacade(
            User model,
            PlayerView view,
            MoveProjectiles pMover,
            SpawnProjectiles pSpawner)
        {
            _view = view;
            _model = model;
            _view.Facade = this;
            _health = new PlayerHealth(_view.Animator, _view.gameObject, null);
            _movement = new PlayerMovement(_view.transform, _view.Animator, _view.Rigidbody, _view.Controller);
            _shooting = new PlayerShooting(_view.Animator, _view.FirePoint, _view.GunFireSource, _view.MuzzleParticle, pMover, pSpawner);
        }

        public void CustomUpdate()
        {
            _movement.CustomUpdate();
            _shooting.CustomUpdate();
        }

        public void CustomFixedUpdate()
        {
            _movement.CustomFixedUpdate();
        }

        public void Initialize()
        {

        }
    }
}
