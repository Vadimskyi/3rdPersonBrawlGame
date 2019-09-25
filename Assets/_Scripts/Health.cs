using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<int> OnDamageTaken = delegate {  };
    public int MaxHealth => _startingHealth;
    public int CurrentHealth => _currentHealth;

    [SerializeField]
    private int _startingHealth = 10;
    [SerializeField]
    private ParticleSystem _damageParticle;
    [SerializeField]
    private Animator _animator;

    private int _currentHealth;

    public void TakeDamage(int dmgAmount)
    {
        _currentHealth -= dmgAmount;
        OnDamageTaken?.Invoke(dmgAmount);

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
        _animator.SetBool("dead", true);
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
