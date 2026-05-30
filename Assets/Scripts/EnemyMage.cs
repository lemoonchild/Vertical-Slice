using UnityEngine;
using UnityEngine.AI;

public class EnemyMage : MonoBehaviour
{
    [Header("Stats")]
    public float maxHP = 60f;
    public float attackDamage = 8f;
    public float attackCooldown = 2f;

    [Header("Distancia")]
    [Tooltip("Distancia ideal que el mago intenta mantener respecto al player.")]
    public float preferredDistance = 6f;
    [Tooltip("Distancia máxima a la que puede disparar.")]
    public float attackRange = 8f;
    public float detectionRange = 12f;
    [Tooltip("Margen para no estar corrigiendo posición todo el tiempo cerca de preferredDistance.")]
    public float distanceBuffer = 0.5f;

    [Header("Proyectil")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 8f;

    [Header("Pathing")]
    public float repathInterval = 0.15f;

    [Header("UI")]
    public GameObject healthBarPrefab;
    private EnemyHealthBar healthBar;

    private float currentHP;
    private float attackTimer = 0f;
    private float repathTimer = 0f;
    private Transform player;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHP = maxHP;
        attackTimer = Random.Range(0f, attackCooldown);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning($"{name}: no se encontró ningún objeto con tag 'Player'.");

        agent.avoidancePriority = Random.Range(30, 70);

        agent.enabled = false;
        Invoke(nameof(EnableAgent), 0.5f);

        if (healthBarPrefab != null)
        {
            GameObject canvas = GameObject.Find("Canvas");
            GameObject bar = Instantiate(healthBarPrefab, canvas.transform);
            healthBar = bar.GetComponent<EnemyHealthBar>();
            healthBar.target = transform;
        }
    }

    private void EnableAgent()
    {
        agent.enabled = true;
    }

    private void Update()
    {
        if (player == null || !agent.enabled || !agent.isOnNavMesh) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > detectionRange)
        {
            HoldPosition();
            return;
        }

        if (distance > attackRange)
        {
            // Demasiado lejos: acercarse.
            ChasePlayer();
        }
        else if (distance < preferredDistance - distanceBuffer)
        {
            // Demasiado cerca: alejarse (kiting).
            RetreatFromPlayer();
            TryShoot();
        }
        else
        {
            // En la zona buena: quedarse quieto y disparar.
            HoldPosition();
            TryShoot();
        }
    }

    private void ChasePlayer()
    {
        if (agent.isStopped) agent.isStopped = false;

        repathTimer -= Time.deltaTime;
        if (repathTimer <= 0f)
        {
            SetDestinationOnMesh(player.position);
            repathTimer = repathInterval;
        }
    }

    private void RetreatFromPlayer()
    {
        if (agent.isStopped) agent.isStopped = false;

        Vector3 away = (transform.position - player.position).normalized;
        Vector3 target = transform.position + away * preferredDistance;
        SetDestinationOnMesh(target);
    }

    private void HoldPosition()
    {
        // Parar de forma limpia sin pelear con el path.
        if (!agent.isStopped) agent.isStopped = true;
        agent.ResetPath();
    }

    private void SetDestinationOnMesh(Vector3 worldPos)
    {
        // Ajusta el destino al punto válido del NavMesh más cercano.
        if (NavMesh.SamplePosition(worldPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
    }

    private void TryShoot()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0f)
        {
            Attack();
            attackTimer = attackCooldown;
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
            rb.linearVelocity = dir * projectileSpeed;
        }
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