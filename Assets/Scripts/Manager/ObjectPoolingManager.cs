using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager Instance;

    [SerializeField] private GameObject poolingObj;

    Queue<Projectile> poolingObjectQueue = new Queue<Projectile>();

    private void Awake()
    {
        Instance = this;

        Initialize(10);
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    private Projectile CreateNewObject()
    {
        var newObj = Instantiate(poolingObj).GetComponent<Projectile>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static Projectile GetObject(Transform parent)
    {
        if (Instance.poolingObjectQueue.Count > 0)
        {
            var obj = Instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(parent);
            obj.transform.position = parent.position;
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.transform.SetParent(parent);
            newObj.transform.position = parent.position;
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    public static void ReturnObject(Projectile obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.poolingObjectQueue.Enqueue(obj);
    }
}
