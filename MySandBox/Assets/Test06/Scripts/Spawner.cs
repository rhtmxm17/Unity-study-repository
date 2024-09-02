using System.Collections;
using Test06;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    [Header("인스펙터 초기화")]
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject prefab;

    [Space]
    [SerializeField] private float spawnCycle = 3f;
    [SerializeField] private float spawnCycleMin = 0.3f;

    private UnityAction<GameManager.GameState> gameChangeAction;
    private Transform player;
    Coroutine spawnRoutine;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += WhenGameStateChanged;
        player = GameObject.FindWithTag("Player").transform;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateChanged -= WhenGameStateChanged;
    }

    private void WhenGameStateChanged(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.Running:
                spawnRoutine = StartCoroutine(Spawn());
                break;
            case GameManager.GameState.GameOver:
                spawnRoutine = null;
                break;
        }
    }

    private IEnumerator Spawn()
    {
        // StopCoroutine vs 종료조건?
        do
        {
            Vector3 position;
            do
            {
                position = spawnPoints[Random.Range(0, spawnPoints.Length)].position; // 스폰 지점중 무작위 선정
            } while ((player.position - position).sqrMagnitude < 25f); // 너무 가까우면 다시 뽑기

            Quaternion rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up); // 무작위 Y축 회전
            Instantiate(prefab, position, rotation);

            yield return new WaitForSeconds(spawnCycle);
            spawnCycle *= 0.9f;
            if (spawnCycle < spawnCycleMin)
                spawnCycle = spawnCycleMin;
        } while (spawnRoutine != null) ;
    }
}
