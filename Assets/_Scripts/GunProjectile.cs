using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunProjectile : MonoBehaviour
{
    public event Action<GameObject> OnHit = delegate { };

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag.Equals("LocalPlayer")) return;
        OnHit?.Invoke(collider.gameObject);
        Destroy(transform.parent.gameObject);
    }
}
