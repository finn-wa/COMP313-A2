using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{    float health;
    public UnityEvent onDeath;

    // Use this for initialization
    void Start()
    {
        health = 100;
    }

    public void Damage(float amount)
    {
        health -= amount;
		print("Player health = "+health);
        if (health <= 0)
        {
            onDeath.Invoke();
        }
    }
}
