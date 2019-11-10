using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MoveProjectiles : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _range;

    public void MoveProjectile(GameObject projectile, Vector3 direction)
    {
        Vector3 distance = Vector3.zero;
        Observable.EveryFixedUpdate().Subscribe(_ => { UpdateProjectilePos(ref distance, projectile, direction); }).AddTo(projectile);
        UpdateProjectilePos(ref distance, projectile, direction);
    }

    private void UpdateProjectilePos(ref Vector3 distance, GameObject projectile, Vector3 direction)
    {
        distance += direction * (_speed * Time.fixedDeltaTime);
        projectile.transform.position += direction * (_speed * Time.fixedDeltaTime);

        if (Mathf.Abs(distance.magnitude) >= _range)
            Destroy(projectile);
    }
}
