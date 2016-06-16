using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/**
 * Spawns enemies periodically with decreasing time interval randomly within a
 * defined area.
 * TODO: Spawn in a uniform circle around castle instead
 */
public class EnemySpawner : MonoBehaviour {
    private GameObject dudePrefab;
    private GameObject bigDudePrefab;

    // Decreases minimum time to wait and maximum time (in seconds)
    // to wait by this factor after each spawn
    private float difficultyMultiplier = 0.98f;
    private float spawnMinTimer = 3f;
    private float spawnMaxTimer = 4f;
    private float spawnRadius = 3f;

    private int currentLevel = 0;
    private static readonly float LEVEL_DUR = 30f;

    public GameObject levelStartButton;

    // D = Dude; B = BigDude;
    private string[] levelSpecs = { "10D:10B",
                                    "10D:3B" };
    private System.Random rng = new System.Random();

    // Use this for initialization
    void Start () {
        dudePrefab = (GameObject) Resources.Load("Dude");
        bigDudePrefab = (GameObject) Resources.Load("BigDude");
        ShowButton();
        StartNextLevel();
	}
	
    IEnumerator SpawnEnemies(int level) {
        List<char> spawnList = ExpandEnemyStringToRandomizedList(levelSpecs[level]);
        int enemyCount = spawnList.Count;
        float averageWait = LEVEL_DUR / enemyCount;
        float remainingTime = LEVEL_DUR;
        int index = 0;

        while (enemyCount > 0) {
            float waitTime = Random.Range(0, remainingTime / enemyCount);

            yield return new WaitForSeconds(waitTime);
            switch (spawnList[index]) {
                case 'D':
                    Instantiate(dudePrefab, SelectRandomPointHalfCircle(), Quaternion.identity);
                    break;
                case 'B':
                    Instantiate(bigDudePrefab, SelectRandomPointHalfCircle(), Quaternion.identity);
                    break;
                default:
                    Debug.Log("Unknown spawn type");
                    break;
            }

            ++index;
            remainingTime -= waitTime;
            --enemyCount;
        }

        // Wait for enemies to die before displaying level button
        while (true) {
            GameObject[] remainingEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (remainingEnemies.Length == 0) {
                ShowButton();
                break;
            }
            yield return new WaitForSeconds(1);
        }
    }

    private void ShowButton() {
        levelStartButton.SetActive(true);
    }

    private void HideButton() {
        levelStartButton.SetActive(false);
    }

    public void StartNextLevel() {
        if (currentLevel < levelSpecs.Length) {
            HideButton();
            StartCoroutine(SpawnEnemies(currentLevel));
            ++currentLevel;
       } else {
            Debug.Log("There's no next level!");
        }
    }

    private Vector3 SelectRandomPointHalfCircle() {
        Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;

        float x = randomPoint.x + transform.position.x;
        float y = 0.02f; // TODO: Flying units?
        float z = Mathf.Abs(randomPoint.y) + transform.position.z;

        return new Vector3(x, y, z);
    }

    // TODO: Use Enemy Behavior Interface Polymorphism here
    private List<char> ExpandEnemyStringToRandomizedList(string enemyList) {
        string[] seq = enemyList.Split(':');

        List<char> randomizedList = new List<char>();
        int totalCount = 0;
        foreach (string str in seq) {
            string type = Regex.Replace(str, @"[\d-]", "");
            int count = int.Parse(Regex.Replace(str, "[^0-9.]", ""));

            for (int i = 0; i < count; ++i) {
                randomizedList.Add(type[0]);
            }
            totalCount += count;
        }

        Shuffle(randomizedList);

        return randomizedList;
    }

    // http://stackoverflow.com/questions/273313/randomize-a-listt
    private void Shuffle<T>(IList<T> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
