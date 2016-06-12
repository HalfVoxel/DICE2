using UnityEngine;
using System.Collections.Generic;

public class BlackHole : MonoBehaviour {

	static List<BlackHole> BlackHoles = new List<BlackHole>();

	public BlackHole connection;

	public float mass = 1;

	public const float GraviationalConstant = 200;

	public void OnTriggerEnter2D (Collider2D coll) {
		if (connection != null) {
			var ship = coll.transform.GetComponent<Spaceship>();
			if (ship != null) {
				ship.TryTeleportTo(this, connection);
			}

			var asteroid = coll.transform.GetComponent<Asteroid>();
			if (asteroid != null) {
				asteroid.TryTeleportTo(this, connection);
			}
		}
	}

	void Update () {
		if (transform.position.x < Camera.main.transform.position.x - 400) {
			GameObject.Destroy(gameObject);
		}
	}

	void OnDrawGizmos () {
		if (connection != null) {
			Gizmos.color = Color.red;
			Gizmos.DrawLine(transform.position, connection.transform.position);
		}
	}

	public static Vector2 AccelerationAtPoint (Vector2 p) {
		Vector2 acc = Vector2.zero;
		for (int i = 0; i < BlackHoles.Count; i++) {
			var dir = (Vector2)BlackHoles[i].transform.position - p;
			var dist = dir.magnitude;
			dist = Mathf.Max(dist, 8);
			acc += dir.normalized * GraviationalConstant * BlackHoles[i].mass / dist;
		}
		return acc;
	}

	void OnEnable () {
		BlackHoles.Add(this);
	}

	void OnDisable () {
		BlackHoles.Remove(this);
	}
}
