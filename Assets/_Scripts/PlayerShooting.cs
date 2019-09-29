using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Vadimskyi.Game
{
    public class PlayerShooting
    {
        public event Action OnFire = delegate { };

        [SerializeField]
        [Range(0.1f, 1.5f)]
        private float _fireRate = 0.5f;

        [SerializeField]
        [Range(1, 10)]
        private int _damage = 3;


        private float _timer;
        private Animator _animator;
        private Transform _firePoint;
        private AudioSource _gunFireSource;
        private ParticleSystem _muzzleParticle;
        private MoveProjectiles _projectileMover;
        private SpawnProjectiles _projectileSpawner;

        public PlayerShooting(
            Animator animator,
            Transform firePoint,
            AudioSource gunFireSource,
            ParticleSystem muzzleParticle,
            MoveProjectiles projectileMover,
            SpawnProjectiles projectileSpawner)
        {
            _animator = animator;
            _firePoint = firePoint;
            _gunFireSource = gunFireSource;
            _muzzleParticle = muzzleParticle;
            _projectileMover = projectileMover;
            _projectileSpawner = projectileSpawner;
        }

        public void CustomUpdate()
        {
            _timer += Time.deltaTime;

#if UNITY_EDITOR
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
#else
            var horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            var vertical = CrossPlatformInputManager.GetAxis("Vertical");
#endif
            if (_timer >= _fireRate
#if UNITY_EDITOR
            && Input.GetButton("Fire1"))
#else
            && CrossPlatformInputManager.GetButton("Fire"))
#endif
            {
                _timer = 0;
                OnFire?.Invoke();
            }
            else
            {
                _animator.SetBool("shooting", false);
            }
        }

        public void FireGun()
        {
            Debug.DrawRay(_firePoint.position, _firePoint.forward * 100, Color.red, 200f, false);
            if (!_firePoint) return;
            if (_muzzleParticle)
                _muzzleParticle.Play();
            _gunFireSource.Play();
            _animator.SetBool("shooting", true);

            var bullet = _projectileSpawner.SpawnProjectile(_firePoint);
            _projectileMover.MoveProjectile(bullet, _firePoint.forward);
            TraceProjectile(bullet);
        }

        private void TraceProjectile(GameObject projectile)
        {
            var com = projectile.GetComponentInChildren<GunProjectile>();
            if (com != null)
            {
                com.OnHit += target =>
                {
                    target?.GetComponent<PlayerView>()?.Facade.Health.TakeDamage(_damage);
                };
            }
        }

        private void FireGun_firstPerson()
        {
            if (!_firePoint) return;
            if (_muzzleParticle)
                _muzzleParticle.Play();
            _gunFireSource.Play();

            Ray ray = Camera.main.ViewportPointToRay(Vector3.one * 0.5f);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 100))
            {
                hitInfo.collider.GetComponent<PlayerView>()?.Facade.Health.TakeDamage(_damage);
            }
        }
    }
}
