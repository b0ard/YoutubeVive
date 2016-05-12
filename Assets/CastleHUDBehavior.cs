using UnityEngine;
using System.Collections;

public class CastleHUDBehavior : MonoBehaviour {
    private TextMesh textMesh;
    private CastleBehavior castle;
    private string GAME_OVER_MSG = "THANKS OBAMA";

	// Use this for initialization
	void Start () {
	    textMesh = GetComponent<TextMesh>();
        castle = GameObject.FindWithTag("Castle").GetComponent<CastleBehavior>();
	}

    // Update is called once per frame
    void Update() {
        // Rotate the mesh so it faces the player's head
        textMesh.transform.LookAt(Camera.main.transform.position);

        if (castle) {
            textMesh.text = "HP: " + castle.GetHitPoints();
        } else {
            // Castle is null, ie. destroyed, display game over message
            textMesh.text = GAME_OVER_MSG;
        }
	}
}
