using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : SingletonMonoBehaviour<CameraController>
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
		transform.position = Vector3.Lerp (transform.position, target.position + new Vector3 (0, 0, -10), Time.deltaTime * 3);
	}

	public void ChangeTarget (Transform newTarget)
	{
		target = newTarget;
	}
}
