using UnityEngine;
using System.Collections;

public class PlatformGenerator : MonoBehaviour {

	public Transform player;

	//private float minVel = 5;
	//private float maxVel = 10;

	private float minDisp = 4;

	public Transform PlatformPrefab;
	public int numPlatformsPerIteration = 5;

	private GameObject temp;

	public int raycastSteps = 10;
	private Transform[] platforms;
	private float yValueForEnemies = 60;
	private float yValueForGun = 45;

	private float gunForce = 1000;
	public Material newMaterial;
	public Material oldMaterial;

	public Transform[] enemies;
	public Transform gun;
	public Transform Bullet;

	private bool playerHasGun = false;
	private bool hasTriggered = false;

	void Start(){
		temp = new GameObject ();
		platforms = new Transform[numPlatformsPerIteration];
	}

	void Update(){
		if (playerHasGun && Cardboard.SDK.Triggered && !hasTriggered) {
			Vector3 forw = Camera.main.transform.forward;
			Transform bullet = Instantiate (Bullet, Camera.main.transform.position + forw, Quaternion.identity) as Transform;
			Rigidbody rigidbody = bullet.GetComponent<Rigidbody> ();
			//rigidbody.useGravity = false;

			rigidbody.velocity = player.GetComponent<Rigidbody>().velocity;
			rigidbody.AddForce (forw * gunForce);
			hasTriggered = true;
		} else
			hasTriggered = false;
	}

	public void setPlayerGun(bool set){
		playerHasGun = set;
	}

	public void generateNewPlatform(Transform player, Vector3 position, Vector3 newVel, bool regen){

		clearCurrentPlatforms ();

		Vector3 tfor = player.forward;
		Vector3 velocity = newVel;


		Vector3 impactPoint = Vector3.zero;

		temp.transform.rotation = Quaternion.LookRotation (new Vector3 (tfor.x, 0, tfor.z), Vector3.up);
		Transform tTrans = temp.transform;

		Vector3 localVel = tTrans.InverseTransformDirection (velocity);
		
		float yMax = .5f * (localVel.y * localVel.y) / -Physics.gravity.y;

		if (getImpactPosFromVelocity (player, newVel, yMax, ref impactPoint)) {

			for (int i = 0; i < numPlatformsPerIteration; i++) {
	
				float randYDisp = Random.Range (minDisp, yMax - 10);

				float xPos = getXCoordFromVel (player.forward, velocity, randYDisp);

				tTrans.position = impactPoint;
			
				temp.transform.Rotate (Random.Range (360 / numPlatformsPerIteration * i, 360 / numPlatformsPerIteration * (i + 1)) * Vector3.up);

				tTrans.Translate (Vector3.forward * xPos);

				Vector3 finalPos = tTrans.position + Vector3.up * randYDisp;

				if(Random.Range(0, 4) == 0){
					int index = Random.Range(0, enemies.Length - 1);
					Transform enemy = Instantiate(enemies[index], finalPos + Vector3.up * .5f, Quaternion.identity) as Transform;
					enemy.LookAt(player);
					enemy.eulerAngles = new Vector3(0, enemy.eulerAngles.y, 0);
				}

				if(!playerHasGun && Random.Range(0, 2) == 0){
					Transform enemy = Instantiate(gun, finalPos + Vector3.up * 1.5f, Quaternion.identity) as Transform;
					gun.LookAt(player);
					gun.eulerAngles = new Vector3(0, enemy.eulerAngles.y, 0);
				}


				Transform platform = Instantiate (PlatformPrefab, finalPos, Quaternion.identity) as Transform;

				platform.LookAt (player);

				platform.eulerAngles = new Vector3 (0, platform.eulerAngles.y, 0);

				platform.GetComponent<Renderer>().sharedMaterial = newMaterial;

				platforms[i] = platform;
			}
		}
	}

	void clearCurrentPlatforms(){

		if (platforms [0] == null)
			return;

		for (int i = 0; i < numPlatformsPerIteration; i++)
			platforms[i].GetComponent<Renderer> ().material = oldMaterial;
	}

	bool getImpactPosFromVelocity(Transform player, Vector3 newVel, float yMax, ref Vector3 impactPoint){

		float step = (((float) 1) / raycastSteps) * yMax;

		Vector3 pPos = player.position;
		Vector3 pFor = player.forward;

		RaycastHit ray = new RaycastHit ();

		for (int i = raycastSteps - 1; i > 0; i--) {
			float xCoord1 = getXCoordFromVel(pFor, newVel, step * i);
			float xCoord2 = getXCoordFromVel(pFor, newVel, step * (i - 1));

			Vector3 pos1 = pPos + pFor * xCoord1 + Vector3.up * step * i;
			Vector3 pos2 = pPos + pFor * xCoord2 + Vector3.up * step * (i - 1);
			//print (xCoord1 + " and " + xCoord2);

			float dist = Vector3.Distance(pos1, pos2);

			if(Physics.Raycast(pos1, pos2 - pos1, out ray, dist, 1 << 8)){
				impactPoint = ray.point;
				print ("Will Impact!");
				return true;
			}
		}
		return false;
	}

	float getXCoordFromVel(Vector3 mforward, Vector3 velocity, float yVal){
		
		Vector3 tfor = mforward;

		temp.transform.rotation = Quaternion.LookRotation (new Vector3 (tfor.x, 0, tfor.z), Vector3.up);
		Transform tTrans = temp.transform;
		//tTrans.position = Vector3.zero;
		Vector3 localVel = tTrans.InverseTransformDirection (velocity);
		
		//float yMax = .5f * (localVel.y * localVel.y) / -Physics.gravity.y;
		//print (yVal);
		float time = localVel.y + Mathf.Sqrt (localVel.y * localVel.y - 4 * (.5f * -Physics.gravity.y) * (yVal));
		time /= -Physics.gravity.y;
		
		float xPos = localVel.z * time;

		return xPos;
	}

}
