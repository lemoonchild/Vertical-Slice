using UnityEngine;

public class DebugCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Ogro trigger tocado por: {other.gameObject.name} layer: {other.gameObject.layer}");
    }
}