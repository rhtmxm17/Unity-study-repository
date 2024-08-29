using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool
{
    public abstract void ReturnPool(MonoBehaviour item);
}

[System.Serializable]
public class ObjectPool<T> : ObjectPool where T : MonoBehaviour
{
    [SerializeField] private Stack<MonoBehaviour> pool; // Stack이라선지 인스펙터 창에서 안보임
    [SerializeField] private Transform owner;

    public ObjectPool(T prototype, int size, Transform owner = null)
    {
        this.owner = owner;
        pool = new Stack<MonoBehaviour>(size);
        for (int i = 0; i < size; ++i)
        {
            T clone = MonoBehaviour.Instantiate(prototype);
            PooledObject poolinfo = clone.gameObject.AddComponent<PooledObject>();
            poolinfo.SetPool(this).SetFocus(clone);
            clone.gameObject.SetActive(false);
            clone.transform.parent = owner;
            pool.Push(clone);
        }
    }

    public T PopPool()
    {
        pool.TryPop(out MonoBehaviour pop);
        pop.transform.SetParent(null);
        pop.gameObject.SetActive(true);
        return (T)pop;
    }

    public T PopPool(float lifeTime)
    {
        pool.TryPop(out MonoBehaviour pop);
        if (pop == null)
            return null;
        pop.transform.SetParent(null);
        pop.gameObject.SetActive(true);
        pop.GetComponent<PooledObject>().ReturnReservation(lifeTime);
        return (T)pop;
    }

    public override void ReturnPool(MonoBehaviour item)
    {
        if (this != item.GetComponent<PooledObject>().Pool)
            Debug.LogError("다른 풀에서 생성된 오브젝트가 반환됨");

        item.transform.SetParent(owner);
        item.gameObject.SetActive(false);
        pool.Push(item);
    }

}
