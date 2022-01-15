using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [SerializeField] List<Pool> pools;

    [SerializeField] Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        foreach (Pool p in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < p.size; i++)
            {
                GameObject o = Instantiate(p.prefab);
                o.SetActive(false);
                objectPool.Enqueue(o);
            }

            poolDictionary.Add(p.tag, objectPool);

            if (p == pools[pools.Count - 1])
                GameManager.instance.UpdateGameState(GameState.READY);
        }
    }

    public void SpawnObject(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError("Pool with tag " + tag + " doesn't exist!");
            return;
        }

        GameObject obj = poolDictionary[tag].Dequeue();

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        poolDictionary[tag].Enqueue(obj);
    }
}
