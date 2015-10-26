using UnityEngine;
using System.Collections;

public class BulletDie : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (tryKill ());
	}
	
	IEnumerator tryKill(){
		if (transform.position.y < 0)
			Destroy (gameObject);
		else {
			yield return new WaitForSeconds (1);
			tryKill();
		}
	}
}
