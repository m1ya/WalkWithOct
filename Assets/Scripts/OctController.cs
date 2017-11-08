using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctController : MonoBehaviour
{

	[SerializeField]
	private GameObject player;

	private const float PERSONAL_DISTANCE = 0.7f;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (transform.position.x < player.transform.position.x)
			transform.position = Vector3.Lerp (transform.position, player.transform.position + new Vector3 (-PERSONAL_DISTANCE, 0, 0), Time.deltaTime * 3);
		else
			transform.position = Vector3.Lerp (transform.position, player.transform.position + new Vector3 (PERSONAL_DISTANCE, 0, 0), Time.deltaTime * 3);
	}
}
