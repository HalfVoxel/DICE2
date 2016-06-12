using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

	public float boostSpeed = 10;
	public float boostDuration = 5;
	public float boostCooldown = 20;
	public float duckSpeed = 0;

	float currentBoost = 0;
	float boostTime = 0;

	public Sprite boostActive;
	public Sprite boostInactive;
	public Image boostIcon;
	public Button boostButton;

	public GameObject duck;

	void OnCollisionEnter2D (Collision2D coll) {
		if (coll.transform.GetComponent<Asteroid>() != null && coll.transform.GetComponent<Duck>() == null) {
			Kill();
		}
	}

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

	public void Jump () {
		currentBoost = boostSpeed;
		boostTime = Time.time;
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
		rg.velocity = (Vector2)transform.up * (speed + currentBoost);

		if (Time.time - boostTime > boostDuration) {
			currentBoost = Mathf.Lerp(currentBoost, 0, 4*Time.deltaTime);
		}

		if (Input.GetKeyDown(KeyCode.Space) && boostButton.interactable) {
			Jump();
		}

		if (Time.time - boostTime > boostCooldown) {
			boostIcon.sprite = boostActive;
			boostButton.interactable = true;
		} else {
			boostIcon.sprite = boostInactive;
			boostButton.interactable = false;
		}

		if (Input.GetKeyDown(KeyCode.W)) {
			var go = GameObject.Instantiate(duck, transform.position, Quaternion.identity) as GameObject;
			go.GetComponent<Rigidbody2D>().velocity = transform.up * duckSpeed;
		}
	}
}
