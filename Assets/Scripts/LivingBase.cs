using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class LivingBase : MonoBehaviour
{
    public int health;
    public UnityEvent onDeath;

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
            OnDeath();
    }

    public void OnDeath()
    {
        onDeath.Invoke();
    }
}