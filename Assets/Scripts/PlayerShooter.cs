using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 0.3f;

    private float nextFireTime = 0f;
    private PlayerMovement playerMovement;
    private Animator animator;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (playerMovement.Input.Player.Fireball.WasPressedThisFrame()
            && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Fire()
    {
        animator.SetTrigger("IsAttacking");
        GameObject proj = Instantiate(
            projectilePrefab,
            firePoint.position,
            firePoint.rotation);
    }
}