using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

	public float speed = 4f;
	//********** 開始 **********//
	public float jumpPower = 700;
	//ジャンプ力
	public LayerMask groundLayer;
	//Linecastで判定するLayer
	//********** 終了 **********//
	private Rigidbody2D rigidbody2D;
	//	private Animator anim;
	//********** 開始 **********//
	private bool isGrounded;
	//着地判定
	//********** 終了 **********//

	void Start ()
	{
//		anim = GetComponent<Animator> ();
		rigidbody2D = GetComponent<Rigidbody2D> ();
	}
	//********** 開始 **********//
	void Update ()
	{
		//Linecastでユニティちゃんの足元に地面があるか判定
		isGrounded = Physics2D.Linecast (
			transform.position + transform.up * 1,
			transform.position - transform.up * 0.05f,
			groundLayer);
		//スペースキーを押し、
		if (Input.GetKeyDown ("space")) {
			//着地していた時、
			if (isGrounded) {
				//Dashアニメーションを止めて、Jumpアニメーションを実行
//				anim.SetBool ("Dash", false);
//				anim.SetTrigger ("Jump");
				//着地判定をfalse
				isGrounded = false;
				//AddForceにて上方向へ力を加える
				rigidbody2D.AddForce (Vector2.up * jumpPower);
			}
		}
		//上下への移動速度を取得
		float velY = rigidbody2D.velocity.y;
		//移動速度が0.1より大きければ上昇
		bool isJumping = velY > 0.1f ? true : false;
		//移動速度が-0.1より小さければ下降
		bool isFalling = velY < -0.1f ? true : false;
		//結果をアニメータービューの変数へ反映する
//		anim.SetBool ("isJumping", isJumping);
//		anim.SetBool ("isFalling", isFalling);
	}
	//********** 終了 **********//

	void FixedUpdate ()
	{
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
}