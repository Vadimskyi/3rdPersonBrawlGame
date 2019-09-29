using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Vadimskyi.Game
{
    public class PlayerHealth
    {
        public event Action<int> OnDamageTaken = delegate { };
        public int MaxHealth => _startingHealth;
        public int CurrentHealth => _currentHealth;

        private Animator _animator;
        private GameObject gameObject;
        private ParticleSystem _damageParticle;

        private int _currentHealth;
        private int _startingHealth = 10;


        public PlayerHealth(
            Animator animator,
            GameObject target,
            ParticleSystem damageParticle)
        {
            _animator = animator;
            gameObject = target;
            _damageParticle = damageParticle;
        }

        public void TakeDamage(int dmgAmount)
        {
            _currentHealth -= dmgAmount;
            OnDamageTaken?.Invoke(dmgAmount);

            if (_damageParticle)
            {
                _damageParticle.Play();
                Observable.Timer(TimeSpan.FromSeconds(_damageParticle.main.duration)).Subscribe(_ => _damageParticle.Stop())
                    .AddTo(gameObject);
            }

            if (_currentHealth <= 0)
                Die();
        }

        private void Die()
        {
            //Observable.Timer(TimeSpan.FromSeconds(_deathParticle.main.duration))
            //    .Subscribe(_ => gameObject.SetActive(false)).AddTo(this);
            _animator.SetBool("dead", true);
            //gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _currentHealth = _startingHealth;
        }
    }
}
