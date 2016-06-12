using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class BlackConnection : MonoBehaviour {

	ParticleSystem particles;
	public BlackHole start;
	public BlackHole end;
	public float thickness = 1;

	void Awake () {
		particles = GetComponent<ParticleSystem>();
	}

	ParticleSystem.Particle[] pts = new ParticleSystem.Particle[1000];

	static float Thickness (float x) {
		return (0.05f + 3 * Mathf.Pow(x - 0.5f, 4)) * (1/0.25f);
	}

	// Update is called once per frame
	void LateUpdate () {
		if (end == null || start == null) {
			GameObject.Destroy(gameObject);
		}

		var count = particles.GetParticles(pts);
		var totalDir = (Vector2)end.transform.position - (Vector2)start.transform.position;
		var normal = new Vector2(-totalDir.y, totalDir.x).normalized;
		for (int i = 0; i < count; i++) {
			var dist = Vector2.Dot((Vector2)pts[i].position - (Vector2)start.transform.position, totalDir.normalized);
			var normalizedDist = dist / totalDir.magnitude;
			var radius = thickness * Thickness(normalizedDist) * (((pts[i].randomSeed * 0.001f) % 1f) - 0.5f);
			pts[i].position = totalDir.normalized * dist + radius * normal + (Vector2)start.transform.position;
			pts[i].velocity = Vector2.Dot(pts[i].velocity, totalDir.normalized) * totalDir.normalized;

			if (normalizedDist > 1) {
				pts[i].lifetime = 0;
			}
		}

		particles.SetParticles(pts, count);
	}
}
