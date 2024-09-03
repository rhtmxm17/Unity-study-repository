using System.Collections.Generic;
using UnityEngine;

public interface IPooling
{
    public GameObject gameObject { get; }

    public void ReadyPopPool();

    public void ReadyPushPool();
}

public class ObjectPool
{
    [SerializeField] private Transform owner = null;
    [SerializeField] private Stack<GameObject> pool;

    public ObjectPool(IPooling prototype, int size)
    {
        GameObject clone = GameObject.Instantiate(prototype.gameObject);
        clone.transform.parent = owner;
        clone.SetActive(false);
        pool.Push(clone);
    }

    public GameObject PopPool()
    {
        pool.TryPop(out GameObject pop);
        if (pop == null)
            return null;
        pop.transform.parent = null;
        pop.SetActive(true);
        return pop;
    }

    public void PushPool(GameObject item)
    {
        item.transform.parent = owner;
        item.SetActive(false);
        pool.Push(item);
    }
}
