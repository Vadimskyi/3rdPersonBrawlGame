using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int _startingHealth = 10;
    [SerializeField]
    private ParticleSystem _damageParticle;

    private int _currentHealth;

    public void TakeDamage(int dmgAmount)
    {
        _currentHealth -= dmgAmount;

        if (_damageParticle)
        {
            _damageParticle.Play();
            Observable.Timer(TimeSpan.FromSeconds(_damageParticle.main.duration)).Subscribe(_ => _damageParticle.Stop())
                .AddTo(this);
        }

        if (_currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        //Observable.Timer(TimeSpan.FromSeconds(_deathParticle.main.duration))
        //    .Subscribe(_ => gameObject.SetActive(false)).AddTo(this);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _currentHealth = _startingHealth;
    }

    private void Update()
    {

    }
}
