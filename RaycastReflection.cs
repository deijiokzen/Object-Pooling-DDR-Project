using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
	[RequireComponent(typeof(LineRenderer))]

	public class RaycastReflection : MonoBehaviour
	{

		public int reflections;
		public float maxLength;

		private LineRenderer lineRenderer;
		private Ray ray;
		private RaycastHit hit;
		//public Camera camera_obj;
		public InputAction fireAction, fireAction2;
		//private GameObject temp;
		public Transform gun_obj;
		//public GameObject playercontroller;
		public LayerMask ignoreLayermask;
		public Material[] Material_Array;
		//private Vector3 Temp_Position, Temp_Forward;
		void Start()
		{
			//RaycastHit screenRayInfo;
			//Physics.Raycast(camera_obj.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0)), out screenRayInfo);
			//transform.LookAt(screenRayInfo.point);
			//a = GetComponent<LineRenderer>().materials;
			lineRenderer = GetComponent<LineRenderer>();
			fireAction.Enable();
			fireAction2.Enable();
		

		}

		// Update is called once per frame
		void Update()
		{
			if (fireAction.IsPressed())
            {

				Fire();
				//temp = playercontroller;

				//Temp_Position = playercontroller.gameObject.transform.position;
				//Temp_Forward = playercontroller.gameObject.transform.forward;
			}
			if(fireAction2.IsPressed())
            {
				lineRenderer.enabled = false;
            }


		}

		void Fire()
		{
			lineRenderer.enabled = true;
			ray = new Ray(gun_obj.position, gun_obj.forward); 
			//lineRenderer.material = a[0];
			//lineRenderer.material.color = Color.HSVToRGB(1f, 1f, 1f); 
			lineRenderer.positionCount = 1;
			lineRenderer.SetPosition(0, gun_obj.position + gun_obj.forward * gun_obj.localScale.x);
			float remainingLength = maxLength;

			for (int i = 0; i < reflections; i++)
			{
				if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLength, ~ignoreLayermask))
				{
					lineRenderer.positionCount += 1;
					lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
					remainingLength -= Vector3.Distance(ray.origin, hit.point);



					if ((false)/*Mathf.Abs(hit.normal.y) < .5*/)
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

					if (hit.collider.tag == "LockWall")
					{
						//lineRenderer.material = a[1];
						//if(lineRenderer.positionCount ==2 )
						lineRenderer.material = Material_Array[3];
						break;
					}
					//if (hit.collider.tag == "ObjectActualizer")
					//{
					//	lineRenderer.material = a[1];
					//	break;
					//}
					if(hit.collider.CompareTag ("ObjectActualizer"))
                    {
						lineRenderer.material = Material_Array[2];
						continue;

					}
					if (hit.collider.CompareTag("Mirror"))
					{

						if (lineRenderer.positionCount == 2)
							lineRenderer.material = Material_Array[1];
						continue;
					}


					else
                    {
						if (lineRenderer.positionCount == 2)
							lineRenderer.material=Material_Array[0];
						break;
                    }
				}
				
			}
		}
		
	}


}