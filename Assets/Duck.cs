using UnityEngine;
using System.Collections;

public class Duck : MonoBehaviour {
	
	void OnCollisionEnter2D (Collision2D coll) {
		if (coll.transform.GetComponent<Asteroid>() != null) {
			GameObject.Destroy(gameObject, 0.1f);
			//GameObject.Destroy(coll.gameObject);
		}
	}

	void Update () {
		var rg = GetComponent<Rigidbody2D>();
		rg.velocity += BlackHole.AccelerationAtPoint(transform.position) * Time.deltaTime;
	}
}
