using UnityEngine;

public class Missile : MonoBehaviour
{
    private float speed = 5f;
    private float lifetime = 4f;

    public void ShootSetting(float speed, Vector3 positon, Quaternion rotation)
    {
        transform.SetLocalPositionAndRotation(positon, rotation);
        this.speed = speed;

        lifetime = 4f;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        lifetime -= Time.deltaTime;
        if (lifetime < 0)
            gameObject.SetActive(false);
    }
}
