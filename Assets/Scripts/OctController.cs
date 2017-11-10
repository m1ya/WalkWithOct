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

	public float speed = 4f;

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
			break;
		case OctStatus.STICK:
			isGrounded = Physics2D.Linecast (
				transform.position + Vector3.right * 0.5f,
				transform.position - Vector3.left * 0.5f,
				groundLayer);
			break;
		}
	}

	void FixedUpdate ()
	{
		if (_status == OctStatus.STICK) {
			if (!isGrounded) {
				transform.eulerAngles = new Vector3 (0, 0, 0);
			}
			if (transform.eulerAngles.z == 90) {
				float y = Input.GetAxisRaw ("Horizontal");
				if (y != 0) {
					_rigidbody.velocity = new Vector2 (_rigidbody.velocity.x, y * speed);
					/*Vector2 temp = transform.localScale;
				temp.y = y;
				transform.localScale = temp;*/
					//			anim.SetBool ("Dash", true);
					Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));
					Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));
					Vector2 pos = transform.position;
					pos.y = Mathf.Clamp (pos.y, min.y + 0.5f, max.y);
					transform.position = pos;
				} else {
					_rigidbody.velocity = new Vector2 (_rigidbody.velocity.x, 0);
					//			anim.SetBool ("Dash", false);
				}
			} else {
				float x = Input.GetAxisRaw ("Horizontal");
				if (x != 0) {
					_rigidbody.velocity = new Vector2 (x * speed, _rigidbody.velocity.y);
					Vector2 temp = transform.localScale;
					temp.x = x;
					transform.localScale = temp;
					//			anim.SetBool ("Dash", true);
					Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));
					Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));
					Vector2 pos = transform.position;
					pos.x = Mathf.Clamp (pos.x, min.x + 0.5f, max.x);
					transform.position = pos;
				} else {
					_rigidbody.velocity = new Vector2 (0, _rigidbody.velocity.y);
					//			anim.SetBool ("Dash", false);
				}
			}
		}
	}

	public void Lifted ()
	{
		transform.eulerAngles = new Vector3 (0, 0, 0);
		_status = OctStatus.LIFTED;
		transform.SetParent (player.transform);
	}

	public bool Putted ()
	{
		transform.SetParent (null);
		if (isGrounded) {
			transform.eulerAngles = new Vector3 (0, 0, 90);
			_status = OctStatus.STICK;
			return false;
		} else {
			_status = OctStatus.CHASE;
			transform.position = player.transform.position + new Vector3 (-PERSONAL_DISTANCE, 0.5f, 0);
			return true;
		}
	}
}
