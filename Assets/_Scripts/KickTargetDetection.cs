using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickTargetDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Trigger!");
    }
}
