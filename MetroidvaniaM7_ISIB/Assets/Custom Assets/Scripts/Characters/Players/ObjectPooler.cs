using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour   // DESIGN PATTERN POOL OBJECT
{
    
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionnary;

    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;                    // DESIGN PATTERN SINGLETON
    }

    #endregion Singleton

    void Start()
    {
        poolDictionnary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionnary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool (string tag, Vector3 position, Quaternion rotation, bool directionRight) //AJOUTER LA VELOCITY
    {
        if (!poolDictionnary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist !");
            return null;
        }
        GameObject objectToSpawn = poolDictionnary[tag].Dequeue();
        objectToSpawn.SetActive(true);

        switch (tag)
        {
            case "FireBall":
                Rigidbody rb = objectToSpawn.GetComponent<Rigidbody>();
                if (directionRight)
                {
                    objectToSpawn.transform.position = position + new Vector3(2.1f,1.15f,0); //avec animation hurt 1.5f,1.88f,0
                    rb.velocity = Vector3.right * 10f;
                }
                else
                {
                    objectToSpawn.transform.position = position + new Vector3(-2.1f, 1.15f, 0);
                    rb.velocity = Vector3.left * 10f;
                }
                break;


            default:
                objectToSpawn.transform.position = position;
                objectToSpawn.transform.rotation = rotation;
                break;
        }
      
        poolDictionnary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }

}
