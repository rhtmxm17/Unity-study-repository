using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShooterPlayerModel : MonoBehaviour, IMovableModel
{
    [Header("Data")]
    [SerializeField] private Vector3 moveSpeed;
    [SerializeField] private bool zoom;
    [SerializeField] private int bullets;

    [Header("Viewer")]
    [SerializeField] private Animator animator;
    [SerializeField] private Image zoomImage;
    [SerializeField] private FPS_FireControl fireControl; // zoom 상태일때만 활성화
    [SerializeField] private TextMeshProUGUI magagineUI;
    [SerializeField] private AudioClip audioClipFire;

    public event UnityAction OnFire;
    public event UnityAction<Vector3> OnMoveSpeedChanged;
    public event UnityAction<bool> OnZoomChanged;
    public event UnityAction<int> OnBulletsChanged;

    private Coroutine fillImageRoutine;

    private void Awake()
    {
        if (animator != null)
        {
            OnMoveSpeedChanged += PresentAnimatorMoveSpeed;
            OnZoomChanged += PresentAnimatorZoom;
        }

        if (fireControl != null)
        {
            OnZoomChanged += PresentFireControlZoom;
        }

        if (zoomImage != null)
        {
            OnZoomChanged += PresentZoomImageZoom;
        }

        if (magagineUI != null)
        {
            OnBulletsChanged += PresentBullet;
        }

        if (audioClipFire != null)
        {
            OnFire += PresentFireAudio;
        }
    }

    public Vector3 MoveSpeed
    {
        get => moveSpeed;
        set { moveSpeed = value; OnMoveSpeedChanged?.Invoke(value); }
    }

    public bool Zoom
    {
        get => zoom;
        set { zoom = value; OnZoomChanged?.Invoke(value); }
    }

    public int Bullets
    {
        get => bullets;
        set { bullets = value; OnBulletsChanged?.Invoke(value); }
    }

    public void TriggerFire()
    {
        OnFire?.Invoke();
    }

    private void PresentAnimatorMoveSpeed(Vector3 speed)
    {
        animator.SetFloat("ZSpeed", speed.z);
        animator.SetFloat("XSpeed", speed.x);
    }

    private void PresentAnimatorZoom(bool zoom) => animator.SetBool("Aim", zoom);

    private void PresentFireControlZoom(bool zoom) => fireControl.ZoomLock = !zoom;
    private void PresentZoomImageZoom(bool zoom)
    {
        if (fillImageRoutine is null)
        {
            fillImageRoutine = StartCoroutine(FillImage());
        }
    }

    private IEnumerator FillImage()
    {
        while (true)
        {
            zoomImage.fillAmount += Time.deltaTime * 5f * (zoom ? 1f : -1f);
            if (zoomImage.fillAmount > 1f)
            {
                zoomImage.fillAmount = 1f;
                break;
            }
            if (zoomImage.fillAmount < 0f)
            {
                zoomImage.fillAmount = 0f;
                break;
            }
            yield return null;
        }
        fillImageRoutine = null;
    }

    private void PresentBullet(int bullets) => magagineUI.SetText("{0}/30", bullets);

    private void PresentFireAudio() => SoundManager.instance.PlaySFX(audioClipFire);
}
