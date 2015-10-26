using UnityEngine;
using System.Collections;

public class EnemyTriggerEnter : MonoBehaviour {

	void OnTriggerEnter(Collider collider){

		if (collider.gameObject.layer == 9 && collider.gameObject.GetComponent<Rigidbody> ().velocity.y < 0) 
			Camera.main.transform.parent.gameObject.GetComponent<PlayerController> ().GameOver ();
		else if (collider.gameObject.layer != 9)
			Destroy (gameObject);
	}
}
