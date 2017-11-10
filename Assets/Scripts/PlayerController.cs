using UnityEngine;
using System.Collections;

public enum PlayerStatus
{
	NORMAL,
	LIFT,
	WAIT
}

public class PlayerController : MonoBehaviour
{
	[SerializeField]
	private OctController octController;

	private PlayerStatus _status;

	public float speed = 4f;
	//********** 開始 **********//
	public float jumpPower = 700;
	//ジャンプ力
	public LayerMask groundLayer;
	public LayerMask octLayer;
	//Linecastで判定するLayer
	//********** 終了 **********//
	private Rigidbody2D rigidbody2D;
	//	private Animator anim;
	//********** 開始 **********//
	private bool isGrounded;
	private bool isOctTouched;
	//********** 終了 **********//

	void Start ()
	{
//		anim = GetComponent<Animator> ();
		_status = PlayerStatus.NORMAL;
		rigidbody2D = GetComponent<Rigidbody2D> ();
	}
	//********** 開始 **********//
	void Update ()
	{
		isGrounded = Physics2D.Linecast (
			transform.position + transform.up * 0.5f,
			transform.position - transform.up * 0.05f,
			groundLayer);
		
		isOctTouched = Physics2D.CircleCast (
			transform.position + transform.up * 0.5f,
			0.8f, Vector2.zero, 0f,
			octLayer);

		//上下への移動速度を取得
		float velY = rigidbody2D.velocity.y;
		//移動速度が0.1より大きければ上昇
		bool isJumping = velY > 0.1f ? true : false;
		//移動速度が-0.1より小さければ下降
		bool isFalling = velY < -0.1f ? true : false;
		//結果をアニメータービューの変数へ反映する
		//		anim.SetBool ("isJumping", isJumping);
		//		anim.SetBool ("isFalling", isFalling);

		if (Input.GetKeyDown (KeyCode.X)) {
			if (!(_status == PlayerStatus.LIFT)) {
				if (isOctTouched)
					_Lift ();
			} else {
				_Put ();
			}
		}

		if (_status == PlayerStatus.WAIT)
			return;

		if (Input.GetKeyDown (KeyCode.Z)) {
			if (isGrounded)
				_Jump ();
		}
	}
	//********** 終了 **********//

	void FixedUpdate ()
	{
		if (_status == PlayerStatus.WAIT)
			return;
		
		float x = Input.GetAxisRaw ("Horizontal");
		if (x != 0) {
			rigidbody2D.velocity = new Vector2 (x * speed, rigidbody2D.velocity.y);
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
			rigidbody2D.velocity = new Vector2 (0, rigidbody2D.velocity.y);
//			anim.SetBool ("Dash", false);
		}
	}

	void _Jump ()
	{
		//Dashアニメーションを止めて、Jumpアニメーションを実行
		//				anim.SetBool ("Dash", false);
		//				anim.SetTrigger ("Jump");
		//AddForceにて上方向へ力を加える
		rigidbody2D.AddForce (Vector2.up * jumpPower);
	}

	void _Lift ()
	{
		Debug.Log ("Lift");
		_status = PlayerStatus.LIFT;
		octController.Lifted ();
	}

	void _Put ()
	{
		if (octController.Putted ())
			_status = PlayerStatus.NORMAL;
		else {
			_status = PlayerStatus.WAIT;
			CameraController.Instance.ChangeTarget (octController.gameObject.transform);
		}
	}
}