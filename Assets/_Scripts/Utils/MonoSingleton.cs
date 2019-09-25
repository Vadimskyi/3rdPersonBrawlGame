using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi
{
    public class MonoSingleton<T> : MonoBehaviour where T: MonoBehaviour
    {
        public static T Instance
        {
            get {
                return _instance 
                         ?? (_instance = Object.FindObjectOfType<T>()) 
                         ?? new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
            }
        }
        private static T _instance;
    }
}
