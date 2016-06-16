using UnityEngine;
using System.Collections;

public class LevelStartButton : InteractableBase {
    bool hoveringOver;
    EnemySpawner enemySpawner;
    bool buttonPressedDownWhileHovering = false;

	// Use this for initialization
	void Start () {
        // EnemyRelated should only be spawner at this point...
	    enemySpawner = GameObject.FindWithTag("EnemyRelated").GetComponent<EnemySpawner>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnTriggerPressUp(WandController wand) {
        enemySpawner.StartNextLevel();
        gameObject.SetActive(false);
    }
}
