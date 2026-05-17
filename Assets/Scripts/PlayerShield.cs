using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [Header("Escudo")]
    public float shieldDuration = 2f;
    public float cooldown = 4f;

    private bool isShielding = false;
    private bool onCooldown = false;
    private float timer = 0f;
    private PlayerMovement playerMovement;
    private Animator animator;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (onCooldown)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
                onCooldown = false;
        }

        if (playerMovement.Input.Player.Shield.WasPressedThisFrame()
            && !isShielding && !onCooldown)
        {
            ActivateShield();
        }

        if (isShielding)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
                DeactivateShield();
        }
    }

    private void ActivateShield()
    {
        isShielding = true;
        timer = shieldDuration;
        animator.SetTrigger("IsShielding");
        Debug.Log("Escudo activado");
    }

    private void DeactivateShield()
    {
        isShielding = false;
        onCooldown = true;
        timer = cooldown;
        Debug.Log("Escudo desactivado");
    }

    public bool IsShielding => isShielding;
}