using UnityEngine;
using System.Collections; // nessesaire pour IEnumerator
using UnityEngine.UI; // pour travailler les UI element.

public class WaveSpawner : MonoBehaviour
{

    public Transform enemyPreFab;

    public Transform spawnPoint;

    public float timeBetweenWaves = 5f; //time betewwen waves.
    private float countdown = 2f; //time before the first wave.

    public Text waveCountdownText; //reference avec le UI element "waveCountdownText"

    private int waveIndex = 0;

    void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave()); //start la Fonctions IEnumerator SpawnWave.
            countdown = timeBetweenWaves;

        }

        countdown -= Time.deltaTime;

        //pour etre certain que le countdown n'arrive jamais a Zero.
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);


        waveCountdownText.text =string.Format("{0:00.00}", countdown);

    }

    IEnumerator SpawnWave ()  //a la place d'une void on a utiliser cela
    {
        //Debug.Log("Wave incomming!!");
        waveIndex++;
        PlayerStats.Rounds++;

        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);

        }          
    }

    void SpawnEnemy()
    {

        Instantiate(enemyPreFab, spawnPoint.position, spawnPoint.rotation);
    }

}
