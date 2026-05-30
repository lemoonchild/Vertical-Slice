using UnityEngine;
using UnityEngine.AI;

public class EnemyOgre : MonoBehaviour
{
    [Header("Stats")]
    public float maxHP = 100f;
    public float attackDamage = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;

    [Header("Detección")]
    public float detectionRange = 10f;

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
            agent.SetDestination(player.position);

        if (distance <= attackRange)
        {
            agent.SetDestination(transform.position); 
            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackCooldown;
            }
        }
    }

    private void Attack()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
            playerHealth.TakeDamage(attackDamage);
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        Debug.Log($"Ogro recibió {amount} daño. HP: {currentHP}");
        if (currentHP <= 0f)
            Die();
    }

    private void Die()
    {
        EnemyHealth health = GetComponent<EnemyHealth>();
        if (health != null)
            health.NotifyDeath();
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