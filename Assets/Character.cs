using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	Rigidbody2D rg;

	public float speed = 1;
	public float jump = 10;
	public float gravity = 10;
	public bool movementEnabled = true;

	public SpriteRenderer renderer;

	public Sprite idle;
	public Sprite running;
	public Sprite jumping;

	public Animator anim;
	float lastTouchedLayers = 0;


	int lastFrameCollision = 0;

	public LayerMask groundLayers;

	public void Awake () {
		rg = GetComponent<Rigidbody2D>();
	}

	void OnCollisionStay2D (Collision2D coll) {
		lastFrameCollision = Time.frameCount;
	}

	// Update is called once per frame
	void Update () {
		var x = 0;
		if (Input.GetKey(KeyCode.RightArrow)) {
			x = 1;
		}

		if (Input.GetKey(KeyCode.LeftArrow)) {
			x = -1;
		}

		if (!movementEnabled) {
			x = 0;
		}

		var vel = rg.velocity;
		vel.x = x * speed;

		if (rg.IsTouchingLayers(groundLayers)) {
			lastTouchedLayers = Time.time;
		}

		if (Time.time - lastTouchedLayers < 0.05f) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				vel.y = jump;
			}
		}

		vel.y -= gravity * Time.deltaTime;

		rg.velocity = vel;

		anim.SetFloat("horiz", x);
		anim.SetBool("jump", vel.y > 0.1f);
	}
}
