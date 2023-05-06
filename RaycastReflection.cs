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



}