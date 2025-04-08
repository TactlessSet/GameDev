using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 100;
    void Start()
    {
        
    }


    void Update()
    {
        
    }
    
    public float NewHealth(float curHealth, float damage)
    {
        float newHealth = curHealth - damage;
        return newHealth;
    }
}
