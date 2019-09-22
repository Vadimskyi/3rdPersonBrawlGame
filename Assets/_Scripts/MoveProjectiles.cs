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
        Observable.EveryFixedUpdate().Subscribe(_ =>
        {
            distance += direction * (_speed * Time.deltaTime);
            projectile.transform.position += direction * (_speed * Time.deltaTime);

            if(distance.magnitude >= _range)
                Destroy(projectile);
        }).AddTo(projectile);
    }
}
