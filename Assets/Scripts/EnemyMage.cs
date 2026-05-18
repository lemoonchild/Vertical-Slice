using UnityEngine;
using UnityEngine.AI;

public class EnemyMage : MonoBehaviour
{
    [Header("Stats")]
    public float maxHP = 60f;
    public float attackDamage = 8f;
    public float attackRange = 8f;
    public float attackCooldown = 2f;
    public float damage = 8f;

    [Header("Distancia")]
    public float preferredDistance = 6f;
    public float detectionRange = 12f;

    [Header("Proyectil")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    private float currentHP;
    private float attackTimer = 0f;
    private Transform player;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHP = maxHP;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            // Mirar al jugador
            Vector3 lookDir = player.position - transform.position;
            lookDir.y = 0f;
            transform.rotation = Quaternion.LookRotation(lookDir);

            if (distance < preferredDistance)
            {
                Vector3 retreatDir = transform.position - player.position;
                agent.SetDestination(transform.position + retreatDir.normalized * 3f);
            }
            else if (distance > attackRange)
            {
                agent.SetDestination(player.position);
            }
            else
            {
                agent.SetDestination(transform.position);
            }

            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f && distance <= attackRange)
            {
                Attack();
                attackTimer = attackCooldown;
            }
        }
    }

    private void Attack()
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 dir = (player.position - firePoint.position).normalized;
            rb.linearVelocity = dir * 8f;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        Debug.Log($"Mago recibió {amount} daño. HP: {currentHP}");
        if (currentHP <= 0f)
            Die();
    }

    private void Die()
    {
        Debug.Log("Mago ha muerto.");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            Projectile proj = other.GetComponent<Projectile>();
            if (proj != null)
                TakeDamage(proj.damage);
            Destroy(other.gameObject);
        }
    }
}