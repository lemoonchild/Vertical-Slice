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

    [Header("Pathing")]
    [Tooltip("Cada cuántos segundos se recalcula el destino. Evita llamar a SetDestination en cada frame.")]
    public float repathInterval = 0.15f;

    [Header("UI")]
    public GameObject healthBarPrefab;
    private EnemyHealthBar healthBar;

    private float currentHP;
    private float attackTimer = 0f;
    private float repathTimer = 0f;
    private Transform player;
    private PlayerHealth playerHealth;  
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHP = maxHP;
        attackTimer = Random.Range(0f, attackCooldown);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogWarning($"{name}: no se encontró ningún objeto con tag 'Player'.");
        }

        agent.avoidancePriority = Random.Range(30, 70);
        agent.isStopped = false;

        if (healthBarPrefab != null)
        {
            GameObject canvas = GameObject.Find("Canvas");
            GameObject bar = Instantiate(healthBarPrefab, canvas.transform);
            healthBar = bar.GetComponent<EnemyHealthBar>();
            healthBar.target = transform;
        }
    }

    private void Update()
    {
        if (player == null || !agent.isOnNavMesh) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Fuera del rango de detección: no perseguir.
        if (distance > detectionRange)
        {
            if (!agent.isStopped) agent.isStopped = true;
            return;
        }

        if (distance <= attackRange)
        {
            // En rango de ataque: parar y golpear.
            if (!agent.isStopped) agent.isStopped = true;

            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                Attack();
                attackTimer = attackCooldown;
            }
        }
        else
        {
            // Perseguir, recalculando el destino con throttle (no cada frame).
            if (agent.isStopped) agent.isStopped = false;

            repathTimer -= Time.deltaTime;
            if (repathTimer <= 0f)
            {
                if (NavMesh.SamplePosition(player.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                    agent.SetDestination(hit.position);
                repathTimer = repathInterval;
            }
        }
    }

    private void Attack()
    {
        if (playerHealth != null)
            playerHealth.TakeDamage(attackDamage);
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        if (healthBar != null)
            healthBar.UpdateBar(currentHP, maxHP);
        if (currentHP <= 0f)
            Die();
    }

    private void Die()
    {
        if (healthBar != null)
            Destroy(healthBar.gameObject);
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