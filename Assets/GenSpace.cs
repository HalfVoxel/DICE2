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
