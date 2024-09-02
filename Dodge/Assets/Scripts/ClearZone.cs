using System.Collections;
using UnityEngine;

public class ClearZone : MonoBehaviour
{
    [SerializeField] private Vector3 emergeRangeFrom;
    [SerializeField] private Vector3 emergeRangeTo;
    [SerializeField] private float appearTime = 10f;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += WhenGameStateChanged;
        WhenGameStateChanged(GameManager.Instance.State); // 최초 1회 현재 상태에 따른 설정 필요
    }

    private void OnEnable()
    {
        transform.position = new Vector3(
            Random.Range(emergeRangeFrom.x, emergeRangeTo.x),
            Random.Range(emergeRangeFrom.y, emergeRangeTo.y),
            Random.Range(emergeRangeFrom.z, emergeRangeTo.z)
        );
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnGameStateChanged -= WhenGameStateChanged;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.CompareTag("Player"))
        {
            GameManager.Instance.State = GameManager.GameState.Win;
        }
    }

    private void WhenGameStateChanged(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.Ready:
                gameObject.SetActive(false);
                break;
            case GameManager.GameState.Running:
                GameManager.Instance.EntrustCoroutine(WaitActive());
                break;
            case GameManager.GameState.GameOver:
            case GameManager.GameState.Win:
                this.enabled = false;
                break;
            default:
                break;
        }
    }

    private IEnumerator WaitActive()
    {
        yield return new WaitForSeconds(appearTime);
        if (GameManager.Instance.State == GameManager.GameState.Running)
            gameObject.SetActive(true);
    }
}
