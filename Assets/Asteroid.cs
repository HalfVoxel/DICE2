using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

	public float rotationSpeed = 10;

	float scale;

	void Start () {
		scale = (Random.value - 0.5f)*2;
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate(0, 0, rotationSpeed * scale * Time.deltaTime, Space.Self);
	}
}
