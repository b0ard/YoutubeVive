using UnityEngine;
using System.Collections;

/**
 * Spawns enemies periodically with decreasing time interval randomly within a
 * defined area.
 * TODO: Spawn in a uniform circle around castle instead
 */
public class EnemySpawner : MonoBehaviour {
    private GameObject dudePrefab;

    // Decreases minimum time to wait and maximum time (in seconds)
    // to wait by this factor after each spawn
    private float difficultyMultiplier = 0.98f;
    private float spawnMinTimer = 2f;
    private float spawnMaxTimer = 4f;

	// Use this for initialization
	void Start () {
        StartCoroutine(SpawnEnemies());
        dudePrefab = (GameObject) Resources.Load("Dude");
	}
	
    IEnumerator SpawnEnemies() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(spawnMinTimer, spawnMaxTimer));
            Instantiate(dudePrefab, SelectRandomPoint(), Quaternion.identity);

            // TODO: Implement levels
            spawnMinTimer *= difficultyMultiplier;
            spawnMaxTimer *= difficultyMultiplier;
        }
    }

    Vector3 SelectRandomPoint() {
        Vector2 randomPoint = Random.insideUnitCircle * 1.5f;

        float x = randomPoint.x + transform.position.x;
        float y = 0.1f; // TODO: Flying units?
        float z = randomPoint.y + transform.position.z;

        return new Vector3(x, y, z);
    }
}
