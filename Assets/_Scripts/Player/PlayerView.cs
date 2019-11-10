using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vadimskyi.Game
{
    public class PlayerView : MonoBehaviour
    {
        public PlayerFacade Facade;
        public CharacterController Controller;
        public Animator Animator;
        public Rigidbody Rigidbody;
        public Transform FirePoint;
        public ParticleSystem MuzzleParticle;
        public AudioSource GunFireSource;
        public Collider KickCollider;
    }
}