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

        private GameObject createFunc()
        {
            GameObject obj = Instantiate(projectile, gun_obj.position, gun_obj.rotation);
            obj.GetComponent<CollisionDetector>().SetPool(m_projectilePool);
            obj.layer = 2;
            return obj;
        }

        private void actionOnDestroy(GameObject obj)
        {
            Destroy(obj.gameObject);
        }

        private void actionOnRelease(GameObject obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void actionOnGet(GameObject obj)
        {
            obj.gameObject.SetActive(true);
            obj.transform.position = transform.parent.position;
        }



        // Update is called once per frame
        void Update()
        {
            if(fireAction.IsPressed())
            {
                
                    AudioSource.PlayClipAtPoint(clip[0], transform.position);
                    
                    createFunc();
                    //Debug.Log($"Bullet Lag Activated.");
                    bullet_lag = true;
                    timer = Time.time;
            }
          


        }
        
    }
}