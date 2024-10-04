using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour
{
    [SerializeField] Rigidbody ballPrefab;
    [SerializeField] Slider chargeUI;

    [SerializeField] float maxPower;
    [SerializeField] float chargePerSecond;

    private Coroutine sliderUpdateRoutine;
    private float chargeBeginTime;
    private bool isCharging;

    private void Start()
    {
        chargeUI.maxValue = maxPower / chargePerSecond;
        chargeUI.value = 0f;
    }

    private void Update()
    {
        if (!isCharging && 0 < Input.touchCount)
        {
            isCharging = true;
            ChargeBegin();
        }

        if (isCharging && 0 == Input.touchCount)
        {
            isCharging = false;
            Shoot();
        }    
    }

    public void ChargeBegin()
    {
        chargeBeginTime = Time.time;
        sliderUpdateRoutine = StartCoroutine(UpdateSlider());
    }

    public void Shoot()
    {
        // 충전 시간 확인
        float power = (Time.time - chargeBeginTime) * chargePerSecond;
        if (power > maxPower)
            power = maxPower;

        // 발사
        Rigidbody body = Instantiate(ballPrefab, Camera.main.transform.position, Random.rotation);
        body.AddForce(power * Camera.main.transform.forward, ForceMode.Impulse);

        // UI 갱신
        StopCoroutine(sliderUpdateRoutine);
        chargeUI.value = 0f;
    }

    private IEnumerator UpdateSlider()
    {
        YieldInstruction wait = new WaitForSeconds(0.05f);
        while (chargeUI.value < chargeUI.maxValue)
        {
            chargeUI.value = Time.time - chargeBeginTime;
            yield return wait;
        }
    }
}
