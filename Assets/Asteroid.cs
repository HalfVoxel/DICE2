using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	public float rotationSpeed = 10;

	float scale;

	public BlackHole lastBlackHole;
	public float lastBlackHoleTime;

	void Start () {
		scale = (Random.value - 0.5f)*2;
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate(0, 0, rotationSpeed * scale * Time.deltaTime, Space.Self);

		if (transform.position.x < Camera.main.transform.position.x - 200) {
			GameObject.Destroy(gameObject);
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
}
