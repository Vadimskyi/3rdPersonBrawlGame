using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProjectile : MonoBehaviour
{
    public event Action<GameObject> OnHit = delegate { };

    private void OnTriggerEnter(Collider collider)
    {
        OnHit?.Invoke(collider.gameObject);
        Destroy(gameObject);
    }
}
