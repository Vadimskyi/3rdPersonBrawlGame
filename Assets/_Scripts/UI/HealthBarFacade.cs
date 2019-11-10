using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Vadimskyi.Utils;

namespace Vadimskyi.Game
{
    public class HealthBarFacade : IDisposable
    {
        public ReactiveCommand<HealthBarFacade> OnDispose;
        public HealthBarView View { get; }
        public User User { get; }

        private Vector3 _destination;
        private IDisposable _lerpJob;

        public HealthBarFacade(
            HealthBarView view,
            User model)
        {
            View = view;
            User = model;
            Initialize();
        }

        private void Initialize()
        {
            View.gameObject.SetActive(true);
            View.Name.text = User.Name;
            User.Character.CurrentHealth.Subscribe(_ => UpdateHealth());
            OnDispose = new ReactiveCommand<HealthBarFacade>();
            _destination = View.transform.localPosition;
            _lerpJob = Observable.EveryEndOfFrame().Subscribe(_ => LerpJob());
        }

        public void Dispose()
        {
            OnDispose?.Execute(this);
            _lerpJob?.Dispose();
        }

        public void UpdateHealth()
        {
            View.Health.fillAmount = (float)User.Character.CurrentHealth.Value /
                                     User.Character.MaxHealth;
        }

        public void UpdatePosition(Vector3 position)
        {
            _destination = new Vector3(position.x, position.y + 200, position.z);
        }

        private void LerpJob()
        {
            View.transform.localPosition = _destination;
            //View.transform.localPosition = Vector3.Lerp(View.transform.localPosition, _destination, 0.5f);
        }
    }
}
