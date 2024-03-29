﻿using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Vadimskyi.Utils;

namespace Vadimskyi.Game
{
    public class PlayerFacade
    {
        public event Action<Vector3> OnPushed = delegate { };

        public User User => _model;
        public PlayerKick Kick => _kick;
        public PlayerDash Dash => _dash;
        public PlayerView View => _view;
        public PlayerHealth Health => _health;
        public PlayerMovement Movement => _movement;
        public PlayerShooting Shooting => _shooting;

        private User _model;
        private PlayerKick _kick;
        private PlayerDash _dash;
        private PlayerView _view;
        private PlayerHealth _health;
        private PlayerMovement _movement;
        private PlayerShooting _shooting;
        private GameSettings _settings;

        public PlayerFacade(
            User model,
            PlayerView view,
            MoveProjectiles pMover,
            SpawnProjectiles pSpawner)
        {
            _view = view;
            _model = model;
            _view.Facade = this;
            _settings = Services.Get<GameSettings>();
            _health = new PlayerHealth(model, _view.Animator, _view.gameObject, null);
            _movement = new PlayerMovement(model, _view.transform, _view.Animator, _view.Rigidbody, _view.Controller);
            _shooting = new PlayerShooting(model, _view.Animator, _view.FirePoint, _view.GunFireSource, _view.MuzzleParticle, pMover, pSpawner);
            _kick = new PlayerKick(model, _view.transform, _view.KickCollider, _view.Animator, _view.Rigidbody, _movement);
            _dash = new PlayerDash(_model, _view.transform, _view.Animator, _movement);
        }

        public void CustomUpdate()
        {
            _movement.CustomUpdate();
            _shooting.CustomUpdate();
            _health.CustomUpdate();
            _kick.CustomUpdate();
            _dash.CustomUpdate();
        }

        public void CustomFixedUpdate()
        {
            _movement.CustomFixedUpdate();
            _kick.CustomFixedUpdate();
            _dash.CustomFixedUpdate();
        }

        public void Initialize()
        {

        }

        public void Pushed(Vector3 direction)
        {
            OnPushed?.Invoke(direction);
        }

        public void PushEventReceived(UserPush data)
        {
            _view.Animator.SetBool("stumble", true);
            Observable.Timer(TimeSpan.FromMilliseconds(_settings.AnimationSetting.StumbleAnimationTime)).Subscribe(_ => { _view.Animator.SetBool("stumble", false); });
            if (UserData.Instance.User.Id.Equals(data.TargetId))
                _movement.Push(data.Direction);
        }
    }
}
