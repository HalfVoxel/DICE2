﻿using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

	public ParticleSystem particles;
	public Spaceship player;
	public GameObject goalText;

	public void OnTriggerEnter2D (Collider2D coll) {
		Debug.Log(coll.transform.tag);
		if (coll.transform.CompareTag("Player")) {
			OnReachedGoal();
		}
	}

	public void OnReachedGoal () {
		particles.Play();
		player.movementEnabled = false;
		Time.timeScale = 0;
		goalText.SetActive(true);
	}
}
