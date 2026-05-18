using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Stats")]
    public float maxHP = 100f;
    private float currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        Debug.Log($"Jugador recibió {amount} daño. HP: {currentHP}");
        if (currentHP <= 0f)
            Die();
    }

    private void Die()
    {
        Debug.Log("Jugador murió");
    }

    public void Heal(float amount)
    {
        currentHP = Mathf.Min(currentHP + amount, maxHP);
        Debug.Log($"Jugador curado. HP: {currentHP}");
    }

    public float GetHP() => currentHP;
    public float GetMaxHP() => maxHP;
}