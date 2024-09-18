using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CircleCollider2D))]
public class SuikaBall : MonoBehaviour
{
    [SerializeField] private int grade = 2;
    private Rigidbody2D rigid;
    private CircleCollider2D circleCollider;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        AdjustGrade();
    }

    public void SetGrade(int value)
    {
        grade = value;
        AdjustGrade();
    }

    private void AdjustGrade()
    {
        float rad = grade * 0.5f;
        rigid.mass = rad * rad;
        transform.localScale = rad * Vector3.one;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (false == collision.gameObject.TryGetComponent(out SuikaBall other))
            return;

        // 같은 등급일 경우에만 병합
        if (this.grade != other.grade)
            return;

        this.enabled = false;

        // 둘 중 한쪽에서만 병합을 수행시키기 위함
        if (this.GetInstanceID() < other.GetInstanceID())
        {
            // 물리 처리를 마친 후 삭제시키기 위해
            StartCoroutine(DestroyNextFixedUpdate());
            return;
        }

        grade++;
        AdjustGrade();
        transform.position = Vector3.Lerp(transform.position, other.transform.position, 0.5f);
    }

    private IEnumerator DestroyNextFixedUpdate()
    {
        yield return new WaitForFixedUpdate();
        Destroy(gameObject);
    }
}
