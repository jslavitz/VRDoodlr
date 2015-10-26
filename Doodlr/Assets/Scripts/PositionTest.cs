
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class PositionTest : MonoBehaviour {
	private Text text;
	private float fps = 60;
	public Transform player;
	void Awake() {
		text = GetComponent<Text>();
	}
	
	void LateUpdate() {
	
		text.text =  Mathf.RoundToInt(player.position.y) + " m";
	}
}
