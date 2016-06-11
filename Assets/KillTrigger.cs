using UnityEngine;
using System.Collections;

public class KillTrigger : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D coll) {
		if (coll.transform.CompareTag("Player")) {
			coll.GetComponent<Spaceship>().Kill();
		}
	}
}
