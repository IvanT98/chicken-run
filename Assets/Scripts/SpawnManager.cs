using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float enemySpawnPosX;
    public float enemySpawnPosY;
    public float enemySpawnPosZ;
    public float powerupSpawnPosX;
    public float powerupSpawnPosY;
    public float powerupSpawnPosZ;

    public TextMeshProUGUI waveText;

    private float currentWave = 0;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private GameObject[] powerupPrefabs;
    private GameManager gameManager;

    private void Start() {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Spawn();
    }

    void Spawn() {
        if (gameManager.isGameOver()) {
            return;
        }

        GameObject anyEnemy = GameObject.FindGameObjectWithTag("Enemy");

        if (anyEnemy != null) {
            return;
        }

        NextWave();
        SpawnPowerup();

        for (int i = 0; i < currentWave; i++) {
            SpawnEnemy();
        }
    }

    void SpawnEnemy() {
        bool invertPosX = Random.value > 0.5f;
        Vector3 randomPosition = new Vector3(invertPosX ? enemySpawnPosX : -enemySpawnPosX, enemySpawnPosY, Random.Range(-enemySpawnPosZ, enemySpawnPosZ));

        Instantiate(enemyPrefab, randomPosition, enemyPrefab.transform.rotation);
    }

    void SpawnPowerup() {
        GameObject powerupPrefab = powerupPrefabs[Random.Range(0, powerupPrefabs.Length)];
        Vector3 randomPosition = new Vector3(Random.Range(-powerupSpawnPosX, powerupSpawnPosX), powerupSpawnPosY, Random.Range(-powerupSpawnPosZ, powerupSpawnPosZ));

        Instantiate(powerupPrefab, randomPosition, powerupPrefab.transform.rotation);
    }

    void NextWave() {
        currentWave++;
        waveText.text = "Wave: " + currentWave;
    }
}
