using UnityEngine;
using System.Collections;

public class CastleBehavior : MonoBehaviour {
    private int hitPoints = 100;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	    if (hitPoints < 0) {
            Destroy(gameObject);
        }
	}

    public void ProcessDamage(int attackPoints) {
        hitPoints -= attackPoints;
    }

    public int GetHitPoints() {
        return hitPoints;
    }

    private void OnDestroy() {
        GameObject[] enemyRelatedObjs = GameObject.FindGameObjectsWithTag("EnemyRelated");

        foreach (GameObject obj in enemyRelatedObjs) {
            Destroy(obj);
        }
    }
}
