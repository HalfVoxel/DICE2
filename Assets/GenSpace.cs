using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenSpace : MonoBehaviour {

	public GameObject[] asteroids;
	public float spacing = 1;
	public float wavyness = 1;
	public float randomness = 1;
	public float frequency = 0.1f;
	public float minimumSeparation = 10;
	List<Vector2> existingAsteroids = new List<Vector2>();

	public GameObject blackHole;
	public GameObject blackHoleConnection;

	class Chunk {
		public Vector2 center;
		public GenSpace parent;

		public void Generate () {
			for (float x = 0; x < parent.chunkSize; x += parent.spacing * Random.value) {
				var type = Random.Range(0, parent.asteroids.Length);
				var p = new Vector2(x - parent.chunkSize*0.5f, 0) + center;
				p.y += parent.wavyness * Mathf.PerlinNoise(p.x*parent.frequency, p.y);
				p.y += parent.randomness * (Random.value - 0.5f)*2;
				var scale = Random.Range(0.5f, 1f);

				bool colliding = false;
				for (int i = 0; i < parent.existingAsteroids.Count; i++) {
					if ((parent.existingAsteroids[i] - p).sqrMagnitude < scale*parent.minimumSeparation*parent.minimumSeparation) {
						colliding = true;
						break;
					}
				}

				if (colliding) {
					continue;
				}

				var go = GameObject.Instantiate(parent.asteroids[type], p, Quaternion.Euler(0, 0, Random.Range(0f, 360f))) as GameObject;
				go.transform.localScale = Vector3.one * scale;
				parent.existingAsteroids.Add(p);
			}

			for (int i = 0; i < 1; i++) {
				var p = new Vector2(Random.value*parent.chunkSize - parent.chunkSize*0.5f, Random.Range(0.2f, 0.5f) * parent.chunkSize) + center;
				var b1 = GameObject.Instantiate(parent.blackHole, p, Quaternion.identity) as GameObject;
				p = new Vector2(Random.value*parent.chunkSize - parent.chunkSize*0.5f, -Random.Range(0.2f, 0.5f) * parent.chunkSize) + center;
				var b2 = GameObject.Instantiate(parent.blackHole, p, Quaternion.identity) as GameObject;

				var hole1 = b1.GetComponent<BlackHole>();
				var hole2 = b2.GetComponent<BlackHole>();
				hole1.connection = hole2;
				hole2.connection = hole1;

				var conn = GameObject.Instantiate(parent.blackHoleConnection, b1.transform.position, Quaternion.LookRotation(Vector3.forward, b2.transform.position - b1.transform.position)) as GameObject;
				var connComp = conn.GetComponentsInChildren<BlackConnection>();
				foreach (var c in connComp) {
					c.start = hole1;
					c.end = hole2;
				}
			}
		}
	}

	Dictionary<Vector2, Chunk> chunks = new Dictionary<Vector2, Chunk>();

	public Transform player;
	public float chunkSize = 100;
	
	public void Update () {
		Vector2 v0 = new Vector2(Mathf.Round(player.position.x / chunkSize), Mathf.Round(player.position.y / chunkSize));

		for (int dx = -1; dx <= 1; dx++) {
			for (int dy = -1; dy <= 1; dy++) {
				var v = v0 + new Vector2(dx, dy);
				if (!chunks.ContainsKey(v)) {
					chunks[v] = new Chunk() {
						center = v * chunkSize,
						parent = this
					};
					chunks[v].Generate();
				}
			}
		}
	}
}
