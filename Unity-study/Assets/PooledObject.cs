using System.Collections;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
    // ObjectPool에서 사용하기 위한 일종의 신분증 + 유틸

    public ObjectPoolOld Pool { get; private set; }
    public MonoBehaviour Focus { get; private set; }

    private Coroutine reservation;

    public void ReturnReservation(float time)
    {
        if (reservation == null)
        {
            reservation = StartCoroutine(ReturnRoutine(time));
        }
    }

    private IEnumerator ReturnRoutine(float Time)
    {
        yield return new WaitForSeconds(Time);
        Pool.ReturnPool(Focus);
        reservation = null;
    }

    public PooledObject SetPool(ObjectPoolOld pool)
    {
        this.Pool = pool;
        return this;
    }

    public PooledObject SetFocus(MonoBehaviour focus)
    {
        this.Focus = focus;
        return this;
    }

    private void OnDisable()
    {
        if (reservation != null)
        {
            StopCoroutine(reservation);
            reservation = null;
        }
    }
}
