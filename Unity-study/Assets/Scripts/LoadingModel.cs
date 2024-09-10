using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingModel : MonoBehaviour
{
    [SerializeField] private string nextSceneName; // 불러올 씬을 등록
    [SerializeField] private float checkProcessPeriod = 0.1f; // 슬라이더 갱신 주기

    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI loadingText;

    private WaitForSeconds waitCheckProcess;

    private void Awake()
    {
        waitCheckProcess = new WaitForSeconds(checkProcessPeriod);
    }

    private void Start()
    {
        loadingSlider.value = 0f;
        StartCoroutine(WaitAnyInputForStartLoading());
    }

    // 실제라면 필요 없는 동작이겠지만 테스트 작동 확인을 위해 단계를 추가
    private IEnumerator WaitAnyInputForStartLoading()
    {
        loadingText.SetText("아무 키나 누르면 로딩을 시작합니다.");

        // 키 입력 전까지 대기
        while (Input.anyKeyDown == false)
            yield return null;

        loadingText.SetText(string.Empty);

        // 로딩 시작
        LoadingManager.Instance.AllowSceneActivation = false;
        LoadingManager.Instance.StartLoadScene(nextSceneName);

        StartCoroutine(CheckLoadingProcess());
    }

    private IEnumerator CheckLoadingProcess()
    {
        // 주기적으로 로딩 진행도 확인 및 슬라이더 갱신
        while (0.9f > (loadingSlider.value = LoadingManager.Instance.Progress))
            yield return waitCheckProcess;

        // 로딩 완료시 처리
        loadingText.SetText("로딩 완료! 아무 키나 누르세요.");

        StartCoroutine(WaitAnyInputForNextScene());
    }

    private IEnumerator WaitAnyInputForNextScene()
    {
        // 키 입력 전까지 대기
        while (Input.anyKeyDown == false)
            yield return null;

        LoadingManager.Instance.AllowSceneActivation = true;
    }
}
