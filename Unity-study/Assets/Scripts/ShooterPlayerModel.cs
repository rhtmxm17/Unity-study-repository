using System.Collections;
using TMPro;
using UnityEngine;
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

    private Coroutine fillImageRoutine;

    public Vector3 MoveSpeed
    {
        get => moveSpeed;
        set { moveSpeed = value; PresentMoveSpeed(); }
    }

    public bool Zoom
    {
        get => zoom;
        set { zoom = value; PresentZoom(); }
    }

    public int Bullets
    {
        get => bullets;
        set { bullets = value; PresentBullet(); }
    }

    private void PresentMoveSpeed()
    {
        animator.SetFloat("ZSpeed", moveSpeed.z);
        animator.SetFloat("XSpeed", moveSpeed.x);
    }

    private void PresentZoom()
    {
        animator.SetBool("Aim", zoom);
        fireControl.ZoomLock = !zoom;
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

    private void PresentBullet()
    {
        magagineUI.SetText("{0}/30", bullets);
    }
}
