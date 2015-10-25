using UnityEngine;
using System.Collections;

public class EnemyTriggerEnter : MonoBehaviour {

	void OnTriggerEnter(Collider collider){

		if (collider.gameObject.layer == 9) 
			Camera.main.transform.parent.gameObject.GetComponent<PlayerController> ().GameOver ();
		else
			Destroy (gameObject);
	}
}
