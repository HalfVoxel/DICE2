using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenSpace : MonoBehaviour {

	public GameObject[] asteroids;

	class Chunk {
		public Vector2 center;
		public GenSpace parent;

		public void Generate () {
			for (float x = 0; x < parent.chunkSize; x++) {
				var p = new Vector2(x - parent.chunkSize*0.5f, Mathf.PerlinNoise(x, 0) + (Random.value - 0.5f)*2) + center;
				GameObject.Instantiate(parent.asteroids[0], p, Random.rotationUniform);
			}
		}
	}

	Dictionary<Vector2, Chunk> chunks = new Dictionary<Vector2, Chunk>();

	public Transform player;
	public float chunkSize = 100;

	public void Update () {
		Vector2 v = new Vector2(Mathf.Round(player.position.x / chunkSize), Mathf.Round(player.position.y / chunkSize));
		if (!chunks.ContainsKey(v)) {
			chunks[v] = new Chunk() {
				center = v * chunkSize,
				parent = this
			};
			chunks[v].Generate();
		}
	}
}
