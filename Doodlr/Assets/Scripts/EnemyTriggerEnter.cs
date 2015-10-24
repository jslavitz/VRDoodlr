using UnityEngine;
using System.Collections;

public class EnemyTriggerEnter : MonoBehaviour {

	void OnTriggerEnter(Collider collider){
		Camera.main.transform.parent.gameObject.GetComponent<PlayerController> ().GameOver ();
	}
}
