using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	[SerializeField]
	private Transform target;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = Vector3.Lerp (target.position, target.position + Vector3.forward * -10, 5);
	}
}
