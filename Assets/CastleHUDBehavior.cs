using UnityEngine;
using System.Collections;

public class CastleHUDBehavior : MonoBehaviour {
    TextMesh textMesh;
    CastleBehavior castle;

	// Use this for initialization
	void Start () {
	    textMesh = GetComponent<TextMesh>();
        castle = GameObject.FindWithTag("Castle").GetComponent<CastleBehavior>();
	}
	
	// Update is called once per frame
	void Update () {
        textMesh.transform.LookAt(Camera.main.transform.position);
        textMesh.text = "HP: " + castle.GetHitPoints();
	}
}
