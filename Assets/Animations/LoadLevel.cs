using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {

	void Update () {
		if (Time.time > 30f) {
			Application.LoadLevel("Level2");
		}
	}
}
