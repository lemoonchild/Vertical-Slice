using UnityEngine;

public enum Stance
{
    None,
    Damage,
    Healing,
    Invisibility,
    Speed
}

public class StanceSystem : MonoBehaviour
{
    public static StanceSystem Instance;

    public Stance currentStance = Stance.None;

    [Header("Configuración")]
    public float damageMultiplier = 2f;
    public float healAmount = 20f;
    public float invisibilityDuration = 3f;
    public float speedMultiplier = 2f;

    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    private float baseSpeed;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
        baseSpeed = playerMovement.moveSpeed;
    }

    public void SetStance(Stance stance)
    {
        RevertStance();
        currentStance = stance;
        Debug.Log($"Posición activa: {stance}");
    }

    private void RevertStance()
    {
        playerMovement.moveSpeed = baseSpeed;
    }

    public void ActivateStanceAbility()
    {
        switch (currentStance)
        {
            case Stance.Damage:
                break;
            case Stance.Healing:
                playerHealth.Heal(healAmount);
                break;
            case Stance.Invisibility:
                StartCoroutine(InvisibilityRoutine());
                break;
            case Stance.Speed:
                StartCoroutine(SpeedRoutine());
                break;
        }
    }

    private System.Collections.IEnumerator InvisibilityRoutine()
    {
        Debug.Log("Invisible!");
        yield return new WaitForSeconds(invisibilityDuration);
        Debug.Log("Visible de nuevo");
    }

    private System.Collections.IEnumerator SpeedRoutine()
    {
        playerMovement.moveSpeed = baseSpeed * speedMultiplier;
        Debug.Log("Velocidad aumentada!");
        yield return new WaitForSeconds(3f);
        playerMovement.moveSpeed = baseSpeed;
        Debug.Log("Velocidad normal");
    }
}