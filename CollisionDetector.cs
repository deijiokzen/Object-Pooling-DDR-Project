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

        public RaycastReflection ray_ref;
        public void SetPool(IObjectPool<GameObject> pool)
        {
            m_pool = pool;
        }
        private void OnEnable()
        {
            //Ray ray2 = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            //RaycastHit hit;
            //// Check whether your are pointing to something so as to adjust the direction
            //Vector3 targetPoint;
            //if (Physics.Raycast(ray2, out hit, Mathf.Infinity,~ignoreLayermask))
            //    targetPoint = hit.point;
            //else
            //    targetPoint = ray2.GetPoint(1000); // You may need to change this value according to your needs
            if (LockPlatform.isOnPlayer)
            {
                key_activated = true;
            }
            starter_position = game_obj.transform.position;
            GetComponent<Collider>().enabled = false;
            myTailRenderer = GetComponent<TrailRenderer>();
            myTailRenderer.material.color = Color.white;
            //game_obj = GameObject.FindGameObjectsWithTag("MainCamera")[0].transform.Find("Cube").gameObject;
            //game_obj = GameObject.Find("Cube");
            ray = new Ray(RaycastReflection.position_update, RaycastReflection.direction_update);
            //Debug.Log($"FOR THE BULLET: {game_obj.transform.position},{(targetPoint - game_obj.transform.position).normalized}");
            transform.localScale = new Vector3(.25f, .25f, .25f);
            rb = GetComponent<Rigidbody>();
            rb.velocity = RaycastReflection.direction_update * minVelocity;

        }


        private void Update()
        {
            time_to_die -= Time.deltaTime;
            if (time_to_die <= 0 && cubefloat)
            {
                turrentController.hit_detected = true;
                turrentController.hit_position = transform.position;
                cubefloat = false;
               
                //DestructedCube();
                AudioSource.PlayClipAtPoint(clip[0], transform.position);
                m_pool?.Release(gameObject);

            }
            

            //if (cubefloat)
            //{
            //    hitColliders = Physics.OverlapSphere(transform.GetComponent<Renderer>().bounds.center, transform.localScale.x + 3f);
            //    foreach (var hitCollider in hitColliders)
            //    {
            //        Physics.IgnoreCollision(hitCollider, GetComponent<Collider>());
            //    }
            //}

            if (cubefloat && Physics.Raycast(ray.origin, ray.direction, out hit, 20000, ~ignoreLayermask) && hit.transform.CompareTag("ObjectActualizer"))
            {
                actualize = true;
                mirror = false;
                if (Vector3.Distance(GetComponent<Renderer>().bounds.center, hit.point) <= 0.6f)
                {
                    save_actualizer_wall = hit.transform;
                    transform.position = hit.point;
                    Reflector();
                    AudioSource.PlayClipAtPoint(clip[1], transform.position);
                    transform.forward = ray.direction;

                }

            }
            else if (!actualize && hit.transform != null ? hit.transform.CompareTag("Mirror") : false)
            {
                mirror = true;
                if (Vector3.Distance(GetComponent<Renderer>().bounds.center, hit.point) <= 0.6f)
                {

                    transform.position = hit.point;
                    Reflector();
                    AudioSource.PlayClipAtPoint(clip[1], transform.position);
                    transform.forward = ray.direction;

                }
            }
            else if (hit.transform != null ? hit.transform.CompareTag("TeleportPoint") : false)
            {
                if (Vector3.Distance(GetComponent<Renderer>().bounds.center, hit.point) <= 0.6f)
                {

                    GameObject.FindGameObjectsWithTag("Player")[0].transform.position = hit.point;
                    GameObject.FindGameObjectsWithTag("Player")[0].transform.forward = hit.transform.forward;
                    // GameObject.FindGameObjectsWithTag("Player")[0].transform.rotation = Quaternion.identity;
                    Physics.SyncTransforms();

                    AudioSource.PlayClipAtPoint(clip[0], transform.position);
                    m_pool?.Release(gameObject);


                }

            }
            //else if (hit.transform != null ? hit.transform.CompareTag("PrismRotator") : false)
            //{
            //    if (Vector3.Distance(GetComponent<Renderer>().bounds.center, hit.point) <= 0.6f)
            //    {
            //        game_obj = hit.transform.gameObject;
            //        game_obj.GetComponent<PrismRotation>().RotatePrism();
            //        m_pool?.Release(gameObject);


            //    }

            //}
            else if (hit.transform != null ? hit.transform.CompareTag("GravitySwitcher") : false)
            {
                if (Vector3.Distance(GetComponent<Renderer>().bounds.center, hit.point) <= 0.6f)
                {
                    game_obj = hit.transform.gameObject;
                    game_obj.GetComponent<GravitySwitch>().GravityFlip();
                    AudioSource.PlayClipAtPoint(clip[4], transform.position);
                    m_pool?.Release(gameObject);


                }

            }
            else if ((LockPlatform.isOnPlayer||key_activated) && hit.transform != null ? hit.transform.CompareTag("LockWall") : false)
            {
                if (Vector3.Distance(GetComponent<Renderer>().bounds.center, hit.point) <= 0.6f)
                {
                    //DestructedCube();
                    Destroy(hit.transform.gameObject);
                    AudioSource.PlayClipAtPoint(clip[2], starter_position);
                    m_pool?.Release(gameObject);


                }

            }
            //If you're wondering why there are two separate if clauses that use (cubefloat) but functionally could be combined, it's mostly because 
            //I have to ignore collisions in every frame manually since OnCollisionEnter + Physics.IgnoreCollision doesn't disable on impact when switched to discrete for some mysterious reason
            //it's disabling on the frame after it is called, but once it reaches a specifically tagged object it no longer should be doing
            //ray calculations, if you turn cubefloat to false and shoot multiple projectiles in succession they should start colliding with each other

            else
            {

                if (Vector3.Distance(GetComponent<Renderer>().bounds.center, hit.point) < transform.localScale.x + 1f)
                {
                    if (actualize)
                    {
                        //var colliders_to_remove = Physics.OverlapSphere(transform.GetComponent<Renderer>().bounds.center, transform.localScale.x + 3f);
                        //foreach (var hitCollider in colliders_to_remove)
                        //{
                        //    Physics.IgnoreCollision(hitCollider, GetComponent<Collider>(), false);
                        //}
                        //Destroy(this) causes objects to phase to through for some reason even though should logically work 
                        GameObject dice_obj = Instantiate(diceactualizer, transform.position, transform.rotation);


                        dice_obj.transform.SetParent(save_actualizer_wall, true);
                        //dice_obj.transform.parent = save_actualizer_wall;
                        dice_obj.GetComponent<Rigidbody>().AddForce(transform.forward * 3);
                        dice_obj.tag = "Pickable";
                        dice_obj.GetComponent<Collider>().enabled = false;
                        dice_obj.GetComponent<Collider>().enabled = true;
                        AudioSource.PlayClipAtPoint(clip[3], starter_position);
                        m_pool?.Release(gameObject);

                        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        //cube.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                        //cube.transform.position = transform.position;
                        //Rigidbody rb = cube.AddComponent<Rigidbody>();
                        //rb.AddForce(transform.forward * 3);
                        //Destroy(gameObject);

                        //gameObject.GetComponent<Rigidbody>().useGravity = true;
                        //cubefloat = false;
                        //transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                        //gameObject.GetComponent<TrailRenderer>().enabled = false;
                    }
                    else
                    {
                        turrentController.hit_detected = true;
                        turrentController.hit_position = transform.position;
                        //DestructedCube();
                        AudioSource.PlayClipAtPoint(clip[0], transform.position);
                        m_pool?.Release(gameObject);
                        //m_pool?.Release(gameObject);
                    }
                }

            }



        }

        private void Reflector()
        {

            if (hit.transform != null ? hit.transform.name.StartsWith("Prism") : false && Mathf.Abs(hit.normal.y) < .5)
            {
                var previousDirection = ray.direction;
                previousDirection.y = 0;
                var xzNormal = hit.normal;
                xzNormal.y = 0;
                //var direction = Vector3.Reflect(previousDirection, xzNormal);
                ray = new Ray(hit.point, Vector3.Reflect(previousDirection, xzNormal));

            }
            else
            {
                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));

            }
            rb.velocity = ray.direction * minVelocity;


        }

        void DestructedCube()
        {
            if((LockPlatform.isOnPlayer))
            {
                cubesPerAxis = 2;
            }

            else
            {
                cubesPerAxis = 3;

            }
            if(hit.transform.CompareTag("LockWall") && (LockPlatform.isOnPlayer||key_activated))
            {
                cubesPerAxis = 4;
            }
        
            for (int x = 0; x < cubesPerAxis; x++)
            {
                for (int y = 0; y < cubesPerAxis; y++)
                {
                    for (int z = 0; z < cubesPerAxis; z++)
                    {
                        CreateCube(new Vector3(x, y, z));
                    }
                }
            }
            
        }


        void CreateCube(Vector3 clone_coord)
        {
            GameObject actualized_object;
            if (!LockPlatform.isOnPlayer && !key_activated)
            {
                actualized_object = Instantiate(diceactualizer, transform.position, transform.rotation);
                actualized_object.transform.localScale = transform.localScale / cubesPerAxis;
                Vector3 firstcube = transform.position - transform.localScale / 2 + actualized_object.transform.localScale / 2;
                actualized_object.transform.position = firstcube + Vector3.Scale(clone_coord, actualized_object.transform.localScale);
                actualized_object.GetComponent<Rigidbody>().useGravity = true;
                actualized_object.GetComponent<Rigidbody>().AddForce(force * transform.forward);
                Destroy(actualized_object, 3);
            }
            else
            {
                if(hit.transform.CompareTag("LockWall"))
                {
                    actualized_object = Instantiate(lock_wallactualizer, transform.position, Random.rotation);
                    actualized_object.GetComponent<Rigidbody>().AddForce(force * transform.forward);
                    actualized_object.transform.position = transform.position + Vector3.Scale(clone_coord, new Vector3(.2f, .2f, .2f));
                    actualized_object.GetComponent<Rigidbody>().useGravity=false;
                    actualized_object.GetComponent<Rigidbody>().AddForce(force *2f* hit.transform.up);
                }
                else
                {
                    actualized_object = Instantiate(keyactualizer, transform.position, Random.rotation);
                    actualized_object.GetComponent<Rigidbody>().AddForce(force * transform.forward);
                    actualized_object.transform.position = transform.position + Vector3.Scale(clone_coord, new Vector3(.2f, .2f, .2f));
                    Destroy(actualized_object, 3);
                }
                //actualized_object = Instantiate(keyactualizer, transform.position, Random.rotation);
                //actualized_object.GetComponent<Rigidbody>().AddForce(force * transform.forward);
                //actualized_object.transform.position = transform.position + Vector3.Scale(clone_coord, new Vector3(.2f,.2f,.2f));
            }
 
            
            //Destroy(actualized_object, 3);
        }

        //void OnTriggerEnter(Collider collision)
        //{
        //    if (collision.gameObject.CompareTag("Mirror"))
        //    {
        //        //GetComponent<Collider>().enabled = false;
        //        //Physics.IgnoreCollision(collision, GetComponent<Collider>());
        //        Bounce(collision.ClosestPointOnBounds(transform.position).normalized);
        //    }

        //}

        //private void OnCollisionEnter(Collision collision)
        //{
        //    if (collision.gameObject.CompareTag("Mirror"))
        //    {
        //        //collision.gameObject.GetComponent<Collider>().enabled=false;
        //        //GetComponent<Collider>().enabled = false;
        //        Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), gameObject.GetComponent<Collider>());

        //    }
        //}

        //    private void Bounce(Vector3 collisionNormal)
        //    {
        //        rb.velocity = lastFrameVelocity;
        //        var speed = lastFrameVelocity.magnitude;
        //        Vector3 direction;

        //        if (Mathf.Abs(collisionNormal.y) < .5)
        //        {
        //            var previousDirection = lastFrameVelocity.normalized;
        //            previousDirection.y = 0;
        //            var xzNormal = collisionNormal;
        //            xzNormal.y = 0;

        //            direction = Vector3.Reflect(previousDirection, xzNormal);

        //        }
        //        else
        //            direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);



        //        Debug.Log("Out Direction: " + direction);
        //        //rb.AddForce(direction * Mathf.Max(speed, minVelocity));
        //        rb.velocity = direction * minVelocity;
        //    }
    }
}