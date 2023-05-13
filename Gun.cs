using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Pool;
using System;

namespace StarterAssets
{
    public class Gun : MonoBehaviour
    {
        //[SerializeField]
        //private float ForceAmount=70;
        [SerializeField]
        private AudioSource source;
        [SerializeField]
        private AudioClip[] clip;
        [SerializeField]
        private GameObject projectile;
        public Transform gun_obj;
        private IObjectPool<GameObject> m_projectilePool;
        public InputAction fireAction;
        float timer;
        bool bullet_lag;
        [SerializeField]
        private float LagTime=1f;
        //private static bool hasHit = false;

        void Start()
        {
            CollisionDetector.game_obj = gun_obj.gameObject;
            m_projectilePool = new ObjectPool<GameObject>(createFunc, actionOnGet, actionOnRelease, actionOnDestroy, maxSize:5);
            //transform.forward = transform.parent.forward;
            projectile.GetComponent<Rigidbody>().useGravity = false;
            fireAction.Enable();
            bullet_lag = false;
            timer = 0f;
           // this.transform.forward = transform.parent.forward;
        }

}