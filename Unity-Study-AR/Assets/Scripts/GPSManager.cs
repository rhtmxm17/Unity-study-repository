using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSManager : MonoBehaviour
{
    public LocationInfo GPSLocation { get => gpsLocation.lastData; }

    private YieldInstruction waitInitGPS = new WaitForSeconds(1f);
    private LocationService gpsLocation = new();

    private void Start()
    {
        PermissionManager.Request(PermissionManager.FineLocation, StartGPSInit);
    }

    private void StartGPSInit() => StartCoroutine(GPSInit());

    private IEnumerator GPSInit()
    {
        if (false == gpsLocation.isEnabledByUser)
        {
            Debug.LogWarning("GPS가 꺼져있음");
            yield break;
        }

        gpsLocation.Start();

        int waitCount = 10;
        while (gpsLocation.status == LocationServiceStatus.Initializing && waitCount > 0)
        {
            Debug.Log($"GPS 초기화중(잔여 {waitCount}회)");
            waitCount--;
            yield return waitInitGPS;
        }

        switch (gpsLocation.status)
        {
            case LocationServiceStatus.Stopped:
                Debug.Log($"GPS가 정지됨");
                yield break;
            case LocationServiceStatus.Initializing:
                Debug.Log($"GPS 초기화 타임아웃");
                yield break;
            case LocationServiceStatus.Running:
                Debug.Log($"GPS 초기화 완료");
                break;
            case LocationServiceStatus.Failed:
                Debug.Log($"GPS 초기화 실패");
                yield break;
        }

        Debug.Log($"[GPSManager] {GPSLocation.latitude}, {GPSLocation.longitude}");
    }
}
