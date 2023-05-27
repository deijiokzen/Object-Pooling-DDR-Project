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
        int i = 0;
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

            GameObject obj = i>5? m_projectilePool.Get():Instantiate(projectile, gun_obj.position, gun_obj.rotation);
            if (i>5)
            {
                obj.transform.position = gun_obj.position;
                obj.transform.rotation = gun_obj.rotation;
            }
            i++;
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
                //createFunc();
                if (bullet_lag)
                {
                    if (Time.time - timer >= LagTime)
                    {
                        //Debug.Log($"Time: {Time.time} - {timer} = {Time.time-timer}");
                        bullet_lag = false;
                    }

                }
                else
                {
                    AudioSource.PlayClipAtPoint(clip[0], transform.position);

                    createFunc();
                    //Debug.Log($"Bullet Lag Activated.");
                    bullet_lag = true;
                    timer = Time.time;
                }
            }
            //if(bullet_lag)
            //{
            //    if(timer>=2f)
            //    {
            //        timer = 0f;
            //        bullet_lag = false;
            //    }
            //    else
            //    {
            //        timer += Time.deltaTime;
            //    }
            //}
            //else
            //{
            
            //    bullet_lag = true;
            //}
          


        }
        //void Fire()
        //{
        //    createFunc();
            
        //    //GameObject newObj= Instantiate(projectile, transform.parent.position, transform.parent.rotation);
        //    //newObj.transform.parent = gameObject.transform;
        //    //newObj.layer = 2;
        //    //newObj.AddComponent<CollisionDetector>();

        //    //newObj.GetComponent<Rigidbody>().AddForce(transform.parent.forward *ForceAmount);

        //}

        //void OnCollisionEnter(Collision collision)
        //{
        //    Collider myCollider = collision.GetContact(0).thisCollider;
        //    //hasHit = true;

        //}

        //public void CollisionDetected(CollisionDetector childScript)
        //{
        //    Debug.Log("Collided WITH PARENT");

        //    hasHit = true;
        //}
    }
}