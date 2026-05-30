using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public event Action OnDeath;

    public void NotifyDeath()
    {
        OnDeath?.Invoke();
    }
}