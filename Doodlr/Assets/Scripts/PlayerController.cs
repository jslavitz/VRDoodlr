using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public float yVel = 10;
	public float xyVel = 10;

	public float velocityMultiplier = 50;

	private Rigidbody rigidbody;
	public PlatformGenerator platformGenerator;

	private float previousVy;
	private float height;
	public Text gameOverText;
	public GameObject restartText;
	private bool hasEnded = false;

	void Start () {
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		rigidbody = gameObject.GetComponent<Rigidbody> ();
	}

	private Vector3 LaunchPlayer(){
		rigidbody.AddForce (-rigidbody.velocity, ForceMode.VelocityChange);
		Vector3 currentDir = transform.forward;
		currentDir.y = 0;
		currentDir = currentDir.normalized * xyVel;
		currentDir.y = yVel;

		rigidbody.AddForce (currentDir * velocityMultiplier, ForceMode.VelocityChange);

		return currentDir * velocityMultiplier;
	}

	void OnCollisionEnter(Collision platform){
		print (previousVy);

		if (rigidbody == null)
			rigidbody = gameObject.GetComponent<Rigidbody> ();


		if (platform.collider.gameObject.layer == 8 && previousVy <= 0) {
			height = gameObject.transform.position.y;

			Vector3 newVel = LaunchPlayer ();
			platformGenerator.generateNewPlatform(transform, transform.position, newVel, true);
		}
	}

	void Update(){
		if(rigidbody != null)
			previousVy = rigidbody.velocity.y;

		if (gameObject.transform.position.y < height - 4 && !hasEnded) {
			GameOver ();
		}

		if(hasEnded){
			if(Cardboard.SDK.Triggered){
					Application.LoadLevel(0);
			}
		}

	}
	public void GameOver(){
		Destroy (gameObject.GetComponent<Rigidbody>());
		hasEnded = true;	

		gameOverText.gameObject.SetActive (true);
		gameOverText.text = "You climbed " + Mathf.Round (height) + " meters!";
		restartText.SetActive (true);

	}

}
