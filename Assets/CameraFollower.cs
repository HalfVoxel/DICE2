using UnityEngine;
using System.Collections;

public class CameraFollower : MonoBehaviour {

	public static CameraFollower instance;

	public float speed = 1;
	public Transform follow;
	public float lerpSpeed = 1;

	float lerpValue = 1;
	Vector2 lastPos;

	void Awake () {
		instance = this;
	}

	public void StartLerp () {
		lerpValue = 0;
		lastPos = follow.position;
	}

	void LateUpdate () {
		var pos = transform.position;
		pos.x += speed * Time.deltaTime;

		lerpValue += Time.deltaTime * lerpSpeed;
		var targetPos = Vector2.Lerp(lastPos, follow.position, Mathf.SmoothStep(0, 1, lerpValue));
		pos.y = targetPos.y;
		transform.position = pos;
	}
}
