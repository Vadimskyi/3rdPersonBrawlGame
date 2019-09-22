using System;
using System.Collections;
using System.Collections.Generic;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;

public class AggroDetection : MonoBehaviour
{
    public event Action<Transform> OnAggro = delegate {  };

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if(player != null)
        {
            Debug.Log("Aggro!");
            OnAggro(player.transform);
        }
    }
}
