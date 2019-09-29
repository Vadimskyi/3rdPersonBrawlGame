using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        private float _attackRefreshRate = 1.5f;
        [SerializeField]
        private int _attackDamage = 2;

        private AggroDetection _aggroDetection;
        private PlayerHealth _playerHealthTarget;
        private float _attackTimer;

        private void Awake()
        {
            _aggroDetection = GetComponent<AggroDetection>();
            _aggroDetection.OnAggro += AggroDetection_OnAggro;
        }

        private void AggroDetection_OnAggro(Transform target)
        {
            _playerHealthTarget = target.GetComponent<PlayerHealth>();
        }

        private void Update()
        {
            if (_playerHealthTarget != null)
            {
                _attackTimer += Time.deltaTime;
                if (CanAttack())
                    Attack();
            }
        }

        private void Attack()
        {
            _attackTimer = 0;
            _playerHealthTarget?.TakeDamage(_attackDamage);
        }

        private bool CanAttack()
        {
            return _attackTimer >= _attackRefreshRate;
        }
    }
}
