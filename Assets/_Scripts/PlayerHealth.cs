using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Vadimskyi.Game
{
    public class PlayerHealth
    {
        public event Action<int, User> OnTakeDamage = delegate { };
        public event Action<int> OnDeath = delegate { };
        public int MaxHealth => _startingHealth;
        public int CurrentHealth => _currentHealth;

        private User _user;
        private Animator _animator;
        private GameObject gameObject;
        private ParticleSystem _damageParticle;

        private int _currentHealth;
        private int _startingHealth = 10;


        public PlayerHealth(
            User user,
            Animator animator,
            GameObject target,
            ParticleSystem damageParticle)
        {
            _user = user;
            _animator = animator;
            gameObject = target;
            _damageParticle = damageParticle;
            user.Character.OnReset.Subscribe(model =>
            {
                _animator.SetBool("dead", false);
            });
        }

        public void CustomUpdate()
        {
            if (!_user.Character.IsDead
                && gameObject.transform.position.y <
                CompositionRoot.GlobalSettings.ArenaSettings.CharacterFallThreshold)
            {
                NetworkEvents.CharacterTakeDamage(new UserTakeDamage
                {
                    Damage = 100000,
                    UserId = UserData.Instance.User.Id
                });
                //Die();
            }
        }

        public void SendDamageTakenEvent(int dmgAmount, User shooter)
        {
            OnTakeDamage(dmgAmount, _user);
        }

        public void TakeDamage(int dmgAmount)
        {
            _user.Character.CurrentHealth.SetValueAndForceNotify(_user.Character.CurrentHealth.Value - dmgAmount);

            if (_damageParticle)
            {
                _damageParticle.Play();
                Observable.Timer(TimeSpan.FromSeconds(_damageParticle.main.duration)).Subscribe(_ => _damageParticle.Stop())
                    .AddTo(gameObject);
            }

            if (_user.Character.IsDead)
                Die();
        }

        public void Die()
        {
            _user.Character.CurrentHealth.SetValueAndForceNotify(0);
            _animator.SetBool("dead", true);
        }

        private void OnEnable()
        {
            _currentHealth = _startingHealth;
        }
    }
}
