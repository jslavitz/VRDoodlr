using UnityEngine;
using System.Collections;

public class PicksGun : MonoBehaviour {

	void OnTriggerEnter(Collider collider) {
		if (collider.gameObject.layer == 9) {
			GameObject.Find ("Plane").GetComponent<PlatformGenerator> ().setPlayerGun (true);
			Destroy (gameObject);
		}
	}
}
