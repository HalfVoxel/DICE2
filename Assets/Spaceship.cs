using UnityEngine;
using System.Collections;

public class Spaceship : MonoBehaviour {
	
	Rigidbody2D rg;
	
	public float speed = 1;
	public float jump = 10;
	public float gravity = 10;
	public bool movementEnabled = true;
	public float rotationSpeed = 10;

	public Animator anim;

	public GameObject destroyEffect;

	int lastFrameCollision = 0;

	public BlackHole lastBlackHole;
	public float lastBlackHoleTime;

	public void TryTeleportTo (BlackHole from, BlackHole hole) {
		if (lastBlackHole == from && Time.time - lastBlackHoleTime < 1f) {
			// Too soon
			return;
		}

		CameraFollower.instance.StartLerp();
		transform.position = hole.transform.position;
		lastBlackHole = hole;
		lastBlackHoleTime = Time.time;
	}

	public void Awake () {
		rg = GetComponent<Rigidbody2D>();
		rg.velocity = Vector3.right * speed;
	}

	public void Kill () {
		GameObject.Instantiate(destroyEffect, transform.position, transform.rotation);
		gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		var x = 0;
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
			x = -1;
		}
		
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) {
			x = 1;
		}
		
		if (!movementEnabled) {
			x = 0;
		}

		var totalGravity = BlackHole.AccelerationAtPoint(transform.position);
		var newVelocity = rg.velocity + Vector2.Dot(totalGravity, transform.right) * (Vector2)transform.right * Time.deltaTime;
		var angle = (Mathf.Atan2(newVelocity.y, newVelocity.x) - Mathf.PI*0.5f) + x * rotationSpeed * Time.deltaTime;
		transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
		rg.velocity = (Vector2)transform.up * speed;
		Debug.Log(rg.velocity);
	}
}
