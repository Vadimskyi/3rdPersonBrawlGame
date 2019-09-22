using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{
    [SerializeField]
    private Transform _spawnPoint;
    [SerializeField]
    private GameObject _projectile;

    public GameObject SpawnProjectile()
    {
        var projectile = Instantiate(_projectile, _spawnPoint.position, Quaternion.identity);
        projectile.transform.rotation = Quaternion.LookRotation(_spawnPoint.forward);
        projectile.SetActive(true);
        return projectile;
    }
    
}
