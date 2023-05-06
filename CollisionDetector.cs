using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Events;

namespace StarterAssets
{

    using UnityEngine;

    public class CollisionDetector : MonoBehaviour
    {
        public int cubesPerAxis = 3;
        public float delay = 1;
        public float force = 300f;
        public float radius = 2f;
        public GameObject diceactualizer, lock_wallactualizer;
        public GameObject keyactualizer;
        [SerializeField]
        [Tooltip("Just for debugging, adds some velocity during OnEnable")]
        private Vector3 initialVelocity;

        private float minVelocity = 30f;

        private float time_to_die = 10f; //Time to for bullet to die in seconds 

        private TrailRenderer myTailRenderer;

        //private Vector3 lastFrameVelocity;
        private Rigidbody rb;

        public LayerMask ignoreLayermask;

        public static GameObject game_obj; Transform save_actualizer_wall;

        bool key_activated = false;

        bool cubefloat = true, mirror = false, actualize = false;

        Ray ray;
        RaycastHit hit;

        private IObjectPool<GameObject> m_pool;

        private Vector3 starter_position;

        [SerializeField]
        private AudioClip[] clip;

        public void SetPool(IObjectPool<GameObject> pool)
        {
            m_pool = pool;
        }
        private void OnEnable()
        {
            
            if(LockPlatform.isOnPlayer)
            {
                key_activated = true;
            }
            starter_position = game_obj.transform.position;
            GetComponent<Collider>().enabled = false;
            myTailRenderer = GetComponent<TrailRenderer>();
            myTailRenderer.material.color = Color.white;
            //game_obj = GameObject.FindGameObjectsWithTag("MainCamera")[0].transform.Find("Cube").gameObject;
            //game_obj = GameObject.Find("Cube");
            ray = new Ray(game_obj.transform.position, game_obj.transform.forward);
            transform.localScale = new Vector3(.25f, .25f, .25f);
            rb = GetComponent<Rigidbody>();
            rb.velocity = transform.forward * minVelocity;

        }


}