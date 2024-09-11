using UnityEngine;

public class Revolve : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 revolveAxis = Vector3.up;
    [SerializeField] private float revolvePerSecond = 1f;

    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float rotationPerSecond = 1f;

    private float distance;
    private float revolveAngle;
    private float rotationAngle;

    private void Start()
    {
        distance = Vector3.Distance(transform.position, target.position);
        revolveAngle = Vector3.SignedAngle(Vector3.forward, transform.position - target.position, revolveAxis);
        rotationAngle = Vector3.SignedAngle(Vector3.forward, transform.forward, rotationAxis);
    }

    void Update()
    {
        // target의 자식 오브젝트로 넣고 RotateAround로 회전시키는게 훨씬 간단했음
        // 직접 계산해보기를 시도함
        // 결과, 업데이트 순서를 예상할 수 없는 한계로 인한 문제점이 발생했음
        // target보다 위성 오브젝트가 먼저 업데이트되면 위성이 공전한 뒤에 target위치가 변경되서 어색해짐

        revolveAngle += revolvePerSecond * 360f * Time.deltaTime;
        Vector3 newVector = Quaternion.AngleAxis(revolveAngle, revolveAxis) * Vector3.forward * distance;

        rotationAngle += rotationPerSecond * 360f * Time.deltaTime;
        Quaternion newRotation = Quaternion.AngleAxis(rotationAngle, rotationAxis);

        transform.SetPositionAndRotation(target.position + newVector, newRotation);
    }
}
