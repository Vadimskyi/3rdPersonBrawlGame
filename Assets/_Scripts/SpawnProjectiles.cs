using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{
    [SerializeField]
    private GameObject _projectile;

    public GameObject SpawnProjectile(Vector3 pos, Vector3 direction)
    {
        var projectile = Instantiate(_projectile, pos, Quaternion.identity);
        projectile.transform.rotation = Quaternion.LookRotation(direction);
        projectile.SetActive(true);
        return projectile;
    }
    
}
