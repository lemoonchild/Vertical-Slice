using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 15f;
    public float lifetime = 3f;
    public int damage = 10;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.linearVelocity = transform.forward * speed;
        Destroy(gameObject, lifetime);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Collider playerCol = player.GetComponent<Collider>();
            Collider myCol = GetComponent<Collider>();
            if (playerCol != null && myCol != null)
                Physics.IgnoreCollision(myCol, playerCol);
        }
    }
}