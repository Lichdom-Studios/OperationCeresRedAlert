using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    List<string> spawns;
    WaitForSeconds delay;

    ObjectPool pool;

    Quaternion rotation;

    void Start()
    {       
        GameManager.OnGameStateChange += CheckGameState;

        pool = ObjectPool.Instance;

        delay = new WaitForSeconds(1f);

    }

    void CheckGameState(GameState state)
    {
        if (state == GameState.SETUP)
            StartCoroutine(SetupInitialSpawns());
        if(state == GameState.PLAY)
            StartCoroutine(SpawnObjects());
        if (state == GameState.GAMEOVER)
            GameManager.OnGameStateChange -= CheckGameState;
    }

    IEnumerator SpawnObjects()
    {

        while (GameManager.instance.GetGameState() == GameState.PLAY && spawns.Count > 0 )
        {
            yield return delay;
            int rng = Random.Range(0, spawns.Count);

            Vector3 randomPos = new Vector3();

            pool.SpawnObject(spawns[rng], randomPos, rotation, spawns.Count <= 1);

            spawns.RemoveAt(rng);


            yield return null;
        }

        yield break;
    }

    IEnumerator SetupInitialSpawns()
    {

        while (GameManager.instance.GetGameState() == GameState.SETUP && spawns.Count > 0)
        {
            yield return delay;
            int rng = Random.Range(0, spawns.Count);

            Vector3 randomPos = new Vector3();

            pool.SpawnObject(spawns[rng], randomPos, rotation, spawns.Count <= 1);

            spawns.RemoveAt(rng);


            yield return null;
        }

        GameManager.instance.UpdateGameState(GameState.COUNTDOWN);

        yield break;
    }
}
