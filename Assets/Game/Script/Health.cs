using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    private Character _cc;

    private void Awake() 
    {
        maxHealth = currentHealth;
        _cc = GetComponent<Character>();
    }

    public void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " took damage: " + damage);
        Debug.Log(gameObject.name + "'s current health: " + currentHealth);
    }
}
