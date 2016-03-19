using UnityEngine;
using System.Collections;

public class RobotControllerScript : MonoBehaviour {

	public float maxSpeed = 10f;
	private Rigidbody2D rb2D;
	bool facingRight = true;

	Animator anim;

	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	
	public float jumpForce = 350f;
	
	// Use this for initialization
	void Start () {	
		rb2D = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float move = Input.GetAxis ("Horizontal");
		anim.SetFloat ("Speed", Mathf.Abs (move));

		grounded = Physics2D.OverlapCircle (groundCheck.position,
		                                  	 groundRadius,
		                                  	 whatIsGround);
		anim.SetBool ("Ground", grounded);
		anim.SetFloat ("vSpeed", rb2D.velocity.y);
		rb2D.velocity = new Vector2 (move * maxSpeed, rb2D.velocity.y);
		if (grounded) {	
			if (move > 0 && !facingRight) {
				Flip ();
			} else if (move < 0 && facingRight) {
				Flip ();
			}
		}
	}
	
	bool jump = false;
	void Update(){
		if (grounded && Input.GetKeyDown (KeyCode.Space)) {
			anim.SetBool ("Ground", false);
			rb2D.AddForce (new Vector2 (0, jumpForce));
		    jump = true;
		} else {
			if (jump && Input.GetKeyDown (KeyCode.Space)) {
				anim.SetBool("Ground", false);
				rb2D.AddForce (new Vector2 (0, jumpForce));
				jump = false;
			}
		}
		if (grounded && (Input.GetKeyDown (KeyCode.S)|| Input.GetKeyDown(KeyCode.DownArrow))) {
			anim.SetTrigger ("Roll");
		}
	}

	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}

