using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{    
    int health;
    public UnityEvent onDeath;
    public RectTransform healthBar;

    // Use this for initialization
    void Start()
    {
        health = 100;
    }

    public void Damage(int amount)
    {
        health -= amount;
        if (health < 0)
        {
            health = 0;
        }
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
        if (health <= 0)
        {
            onDeath.Invoke();
        }
    }
}
