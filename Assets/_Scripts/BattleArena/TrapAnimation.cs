using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public class TrapAnimation : MonoBehaviour
    {
        public TrapType TrapType;
        
        private Animator _spikeTrapAnim;
        
        void Awake()
        {
            _spikeTrapAnim = GetComponent<Animator>();
        }

        public void LaunchTrap()
        {
            StartCoroutine(OpenCloseTrap());
        }


        IEnumerator OpenCloseTrap()
        {
            _spikeTrapAnim.SetTrigger("open");
            yield return new WaitForSeconds(2);
            _spikeTrapAnim.SetTrigger("close");
        }
    }

    public enum TrapType
    {
        None,
        Spike,
        Door,
        Axes
    }
}
