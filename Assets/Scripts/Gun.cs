using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Gun : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 1.5f)]
    private float _fireRate = 1;
   
    [SerializeField]
    [Range(1, 10)]
    private int _damage = 1;

    [SerializeField]
    private Transform _firePoint;

    [SerializeField]
    private ParticleSystem _muzzleParticle;

    [SerializeField]
    private AudioSource _gunFireSource;

    private float _timer;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Update()
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
            FireGun();
        }
        else
        {
            _animator.SetBool("shooting", false);
        }
    }

    private void FireGun()
    {
        Debug.DrawRay(_firePoint.position, _firePoint.forward * 100, Color.red, 200f, false);
        if (!_firePoint) return;
        if(_muzzleParticle)
            _muzzleParticle.Play();
        _gunFireSource.Play();
        _animator.SetBool("shooting",true);

        Ray ray = new Ray(_firePoint.position, _firePoint.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 100))
        {
            hitInfo.collider.GetComponent<Health>()?.TakeDamage(_damage);
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
            hitInfo.collider.GetComponent<Health>()?.TakeDamage(_damage);
        }
    }
}
