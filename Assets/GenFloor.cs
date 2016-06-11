using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GenFloor : MonoBehaviour {

	public GameObject level0;
	public GameObject level1;
	public GameObject level2;
	public Transform player;

	public Goal goal;
	public Text scoreText;

	List<bool> generated = new List<bool>();
	List<int> height = new List<int>();

	void Start () {
		level0.SetActive(false);
		level1.SetActive(false);
		level2.SetActive(false);

	}

	int lastHoles = -1;

	// Update is called once per frame
	void Update () {
		int idx = (int)player.transform.position.x;
		idx = Mathf.Max(idx, 0);

		int holes = 0;
		for (int i = 0; i < idx; i++) {
			if (height[i] == 0) {
				holes++;
			}
		}

		if (holes > lastHoles) {
			lastHoles = holes;

			scoreText.text = lastHoles.ToString();
			scoreText.GetComponent<Animator>().Play("increase_score");

			if (lastHoles >= 4) {
				goal.transform.position = player.position;
				goal.gameObject.SetActive(true);
			}
		}

		for (int i = idx; i < idx + 10; i++) {
			while (i >= generated.Count) {
				generated.Add(false);
				height.Add(0);
			}

			if (!generated[i]) {
				generated[i] = true;
				var v = Random.value;
				var h = 0;
				if (v < 0.1f) {
					h = 0;
				} else if (v < 0.5f) {
					h = 1;
				} else if (v < 0.8f) {
					h = 2;
				} else {
					h = 3;
				}

				var prevHeight = i > 0 ? height[i-1] : 0;
				h = Mathf.Clamp(h, prevHeight-1, prevHeight+1+1);

				if (prevHeight == 0) {
					h = Mathf.Max(h, 1);
				}

				height[i] = h;

				if (h > 0) {
					var go = GameObject.Instantiate(level0, new Vector2(i, 0), Quaternion.identity) as GameObject;
					go.SetActive(true);
				}

				if (h > 2) {
					var go = GameObject.Instantiate(level2, new Vector2(i, 1), Quaternion.identity) as GameObject;
					go.SetActive(true);
				} else if (h > 1) {
					var go = GameObject.Instantiate(level1, new Vector2(i, 1), Quaternion.identity) as GameObject;
					go.SetActive(true);
				}
			}
		}
	}
}
