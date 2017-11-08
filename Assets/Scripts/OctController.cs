using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OctStatus
{
	CHASE,
	LIFTED,
	STICK
}

public class OctController : MonoBehaviour
{
	private const float PERSONAL_DISTANCE = 0.7f;

	public LayerMask groundLayer;

	[SerializeField]
	private GameObject player;

	private OctStatus _status;
	private Rigidbody2D _rigidbody;

	private bool isGrounded;

	// Use this for initialization
	void Start ()
	{
		_status = OctStatus.CHASE;
		_rigidbody = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		switch (_status) {
		case OctStatus.CHASE:
			if (transform.position.x < player.transform.position.x)
				transform.position = Vector3.Lerp (transform.position, player.transform.position + new Vector3 (-PERSONAL_DISTANCE, 0, 0), Time.deltaTime * 3);
			else
				transform.position = Vector3.Lerp (transform.position, player.transform.position + new Vector3 (PERSONAL_DISTANCE, 0, 0), Time.deltaTime * 3);
			break;
		case OctStatus.LIFTED:
			transform.localPosition = new Vector3 (0, 1.6f, 0);
			isGrounded = Physics2D.Linecast (
				transform.position + Vector3.right * 0.5f,
				transform.position - Vector3.left * 0.5f,
				groundLayer);
			Debug.Log (isGrounded);
			break;
		case OctStatus.STICK:
			Debug.Log ("Stick");
			break;
		}
	}

	public void Lifted ()
	{
		_status = OctStatus.LIFTED;
		transform.SetParent (player.transform);
	}

	public bool Putted ()
	{
		transform.SetParent (null);
		if (isGrounded) {
			_status = OctStatus.STICK;
			return false;
		} else {
			_status = OctStatus.CHASE;
			transform.position = player.transform.position + new Vector3 (-PERSONAL_DISTANCE, 0.5f, 0);
			return true;
		}
	}
}
