using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{
    [SerializeField]
    private GameObject _projectile;

    public GameObject SpawnProjectile(Transform spawnPoint)
    {
        var projectile = Instantiate(_projectile, spawnPoint.position, Quaternion.identity);
        projectile.transform.rotation = Quaternion.LookRotation(spawnPoint.forward);
        projectile.SetActive(true);
        return projectile;
    }
    
}
