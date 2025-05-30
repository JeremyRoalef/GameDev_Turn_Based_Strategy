using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;

    [SerializeField]
    int maxHealth = 100;

    int health = 100;

    private void Awake()
    {
        health = maxHealth;
    }

    public void Damage(int amount)
    {
        health -= amount;

        OnDamaged?.Invoke(this, EventArgs.Empty);

        Debug.Log(health);

        if (health < 0)
        {
            health = 0;
        }

        if (health == 0)
        {
            Die();
        }
    }

    public float GetHealthNormalized()
    {
        return (float)health / maxHealth;
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
}
