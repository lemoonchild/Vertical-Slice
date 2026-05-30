using UnityEngine;
using UnityEngine.AI;

public class EnemyBoss : MonoBehaviour
{
    [Header("Vida")]
    public float maxHP = 200f;
    public float maxShield = 100f;
    public float shieldRegenDelay = 5f;
    public float shieldRegenRate = 20f;

    [Header("Ataque melee")]
    public float attackRange = 2f;
    public float attackDamage = 15f;
    public float attackCooldown = 1.5f;

    [Header("Fase 2")]
    public GameObject ogrePrefab;
    public GameObject magePrefab;
    public Transform[] spawnPoints;

    [Header("Detección")]
    public float detectionRange = 15f;

    [Header("Final")]
    public GameObject catWall; 

    private float currentHP;
    private float currentShield;
    private float attackTimer;
    private float shieldRegenTimer;
    private bool phase2Triggered = false;
    private bool isDead = false;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHP = maxHP;
        currentShield = maxShield;
    }

    private void Update()
    {
        if (isDead || player == null) return;

        RegenerateShield();

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            Vector3 lookDir = player.position - transform.position;
            lookDir.y = 0f;
            if (lookDir.sqrMagnitude > 0.01f)
                transform.rotation = Quaternion.LookRotation(lookDir);

            if (distance > attackRange)
                agent.SetDestination(player.position);
            else
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
    }

    private void RegenerateShield()
    {
        if (currentShield >= maxShield) return;

        shieldRegenTimer -= Time.deltaTime;
        if (shieldRegenTimer <= 0f)
            currentShield = Mathf.Min(currentShield + shieldRegenRate * Time.deltaTime, maxShield);
    }

    private void Attack()
    {
        PlayerHealth health = player.GetComponent<PlayerHealth>();
        if (health != null)
            health.TakeDamage(attackDamage);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        shieldRegenTimer = shieldRegenDelay;

        if (currentShield > 0f)
        {
            currentShield -= amount;
            if (currentShield < 0f)
            {
                currentHP += currentShield;
                currentShield = 0f;
            }
        }
        else
        {
            currentHP -= amount;
        }

        Debug.Log($"Boss — Escudo: {currentShield} HP: {currentHP}");

        if (!phase2Triggered && currentHP <= maxHP * 0.5f)
            TriggerPhase2();

        if (currentHP <= 0f)
            Die();
    }

    private void TriggerPhase2()
    {
        phase2Triggered = true;
        Debug.Log("Boss fase 2 — spawneando refuerzos");

        agent.speed *= 1.5f;
        attackCooldown *= 0.7f;

        if (spawnPoints.Length > 0)
        {
            Instantiate(ogrePrefab, spawnPoints[0].position, Quaternion.identity);
            Instantiate(magePrefab, spawnPoints[1 % spawnPoints.Length].position, Quaternion.identity);
            Instantiate(magePrefab, spawnPoints[2 % spawnPoints.Length].position, Quaternion.identity);
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Boss muerto");
        if (catWall != null)
            catWall.SetActive(false);

        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Boss tocado por: {other.gameObject.name} tag: {other.tag}");
        if (other.CompareTag("Projectile"))
        {
            Projectile proj = other.GetComponent<Projectile>();
            if (proj != null)
                TakeDamage(proj.damage);
            Destroy(other.gameObject);
        }
    }
}