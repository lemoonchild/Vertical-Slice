using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Posición")]
    public Vector3 offset = new Vector3(0f, 12f, -6f);
    public float smoothSpeed = 8f;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position, desiredPos, Time.deltaTime * smoothSpeed);

        transform.LookAt(target.position);
    }
}