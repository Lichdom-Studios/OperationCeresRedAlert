using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] float spawnRadius = 1.5f;
    [SerializeField] float spawnDistance = 10f;
    [SerializeField] int minSpawnAmount = 1, maxSpawnAmount = 5, difficultyChange = 1, maxDifficulty = 20;
    [SerializeField] float difficultyChangeRate = 30f;

    Vector3 lastSpawnPosition = new Vector3(0f,0f,10f);

    [SerializeField] List<string> spawns;
    WaitForSeconds delay;
    WaitForSeconds difficultyWait;

    ObjectPool pool;

    void Start()
    {       
        GameManager.OnGameStateChange += CheckGameState;

        pool = ObjectPool.Instance;

        delay = new WaitForSeconds(1f);
        difficultyWait = new WaitForSeconds(difficultyChangeRate);

        GameManager.instance.UpdateGameState(GameState.SETUP);

    }

    void CheckGameState(GameState state)
    {
        if (state == GameState.SETUP)
            SetupInitialSpawns();
        if (state == GameState.PLAY)
        {
            StartCoroutine(SpawnObjects());
            StartCoroutine(ChangeDifficulty());
        }
        if (state == GameState.GAMEOVER)
            GameManager.OnGameStateChange -= CheckGameState;
    }

    IEnumerator SpawnObjects()
    {

        while (GameManager.instance.GetGameState() == GameState.PLAY)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime * PlayerController.instance.movementSpeed);

            if (Vector3.Distance(lastSpawnPosition, transform.position) >= spawnDistance)
            {
                int rng = Random.Range(0, spawns.Count);

                Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

                pool.SpawnObject(spawns[rng], RandomPositionInCircle(transform.position, spawnRadius), randomRotation);

                lastSpawnPosition = transform.position;
            }
            yield return null;
        }

        yield break;
    }

    void SetupInitialSpawns()
    {
        int numberOfInitialSpawns = Mathf.RoundToInt((transform.position.z - Camera.main.transform.position.z) / spawnDistance);

        for(int i = 0; i < numberOfInitialSpawns; ++i)
        {
            int amount = Random.Range(minSpawnAmount, maxSpawnAmount);
            Vector3 nextPosition = lastSpawnPosition + new Vector3(0, 0, spawnDistance);

            for (int j = 0; j < amount; ++j)
            {
                int rng = Random.Range(0, spawns.Count);

                Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));             

                pool.SpawnObject(spawns[rng], RandomPositionInCircle(nextPosition, spawnRadius), randomRotation);
            }
            lastSpawnPosition = nextPosition;
        }

        GameManager.instance.UpdateGameState(GameState.COUNTDOWN);
    }

    IEnumerator ChangeDifficulty()
    {
        while(GameManager.instance.GetGameState() == GameState.PLAY && maxSpawnAmount < maxDifficulty)
        {
            yield return difficultyWait;
            maxSpawnAmount += difficultyChange;
            minSpawnAmount += difficultyChange;
            yield return null;
        }

        yield break;
    }

    Vector3 RandomPositionInCircle(Vector3 center, float radius)
    {
        float angle = Random.value * 360;
        Vector3 pos;
        //pos.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        //pos.y = center.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad);

        float randomX = Random.Range(-radius, radius);
        float randomY = Random.Range(-radius, radius);

        pos.x = center.x + randomX * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.y = center.y + randomY * Mathf.Cos(angle * Mathf.Deg2Rad);

        pos.z = center.z;

        return pos;
    }
}
