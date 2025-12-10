using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;
    public float DampSpeed = 1.0f;

    private Transform target;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void Update()
    {
        if (target == null)
            return;

        transform.position = Vector3.Lerp(transform.position, target.position + offset, DampSpeed * Time.deltaTime);
    }
}
