using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolOld
{
    [SerializeField] private Stack<GameObject> pool; // Stack이라선지 인스펙터 창에서 안보임
    [SerializeField] private Transform owner;

    public ObjectPoolOld(PooledObject prototype, int size, Transform owner = null)
    {
        this.owner = owner;
        pool = new Stack<GameObject>(size);
        for (int i = 0; i < size; ++i)
        {
            PooledObject clone = MonoBehaviour.Instantiate(prototype);
            clone.SetPool(this);
            clone.gameObject.SetActive(false);
            clone.transform.parent = owner;
            pool.Push(clone.gameObject);
        }
    }

    public GameObject PopPool()
    {
        pool.TryPop(out GameObject pop);
        pop.transform.SetParent(null);
        pop.SetActive(true);
        return pop;
    }

    public GameObject PopPool(float lifeTime)
    {
        pool.TryPop(out GameObject pop);
        if (pop == null)
            return null;
        pop.transform.SetParent(null);
        pop.gameObject.SetActive(true);
        pop.GetComponent<PooledObject>().ReturnReservation(lifeTime);
        return pop;
    }

    public void ReturnPool(GameObject item)
    {
        if (this != item.GetComponent<PooledObject>().Pool)
            Debug.LogError("다른 풀에서 생성된 오브젝트가 반환됨");

        item.transform.SetParent(owner);
        item.gameObject.SetActive(false);
        pool.Push(item);
    }

}
