using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Vadimskyi.Game
{
    public class PlayerShooting
    {
        public event Action OnFire = delegate { };

        private float _timer;
        private User _user;
        private Animator _animator;
        private Transform _firePoint;
        private AudioSource _gunFireSource;
        private ParticleSystem _muzzleParticle;
        private MoveProjectiles _projectileMover;
        private SpawnProjectiles _projectileSpawner;

        public PlayerShooting(
            User user,
            Animator animator,
            Transform firePoint,
            AudioSource gunFireSource,
            ParticleSystem muzzleParticle,
            MoveProjectiles projectileMover,
            SpawnProjectiles projectileSpawner)
        {
            _user = user;
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


            if (_user.Character.IsDead) return;

            if (_timer >= 0.5f
#if UNITY_EDITOR
            && Input.GetButton("Fire1"))
#else
            && CrossPlatformInputManager.GetButton("Fire"))
#endif
            {
                _timer = 0;
                OnFire?.Invoke();
            }
        }

        //TODO: sync bullet spawn point!
        public void FireGun(UserShot data)
        {
            Debug.DrawRay(data.From, data.Direction * 100, Color.red, 200f, false);

            _gunFireSource.Play();
            _animator.SetBool("shooting", true);
            Observable.Timer(TimeSpan.FromMilliseconds(200)).Subscribe(_ =>
            {
                _animator.SetBool("shooting", false);
            });   
            
            var bullet = _projectileSpawner.SpawnProjectile(_firePoint.position, _firePoint.forward);
            bullet.transform.position = _firePoint.position;
            _projectileMover.MoveProjectile(bullet, _firePoint.forward);
            TraceProjectile(bullet, data);

            if (_muzzleParticle)
                _muzzleParticle.Play();
        }

        private void TraceProjectile(GameObject projectile, UserShot data)
        {
            if (!data.UserId.Equals(UserData.Instance.User.Id)) return;
            var com = projectile.GetComponentInChildren<GunProjectile>();
            if (com != null)
            {
                com.OnHit += target =>
                {
                    var view = target?.transform.GetComponent<PlayerView>();
                    var facade = view?.Facade;
                    facade?.Health.SendDamageTakenEvent((int)_user.Character.Damage, facade.User);
                };
            }
        }

        /*private void FireGun_firstPerson()
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
        }*/
    }
}
