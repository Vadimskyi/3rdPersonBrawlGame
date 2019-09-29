using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Vadimskyi.Game
{
    public class EnemyMovement : MonoBehaviour
    {
        //private AggroDetection _aggroDetection;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private Transform _target;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponentInChildren<Animator>();
            // _aggroDetection = GetComponent<AggroDetection>();
            //_aggroDetection.OnAggro += OnAggro;
        }

        private void OnAggro(Transform target)
        {
            _target = target;
        }

        private void Update()
        {
            if (_target)
            {
                _navMeshAgent.SetDestination(_target.position);
                _animator.SetFloat("speed", _navMeshAgent.velocity.magnitude);
            }
        }
    }
}