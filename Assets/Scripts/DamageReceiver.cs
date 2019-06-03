using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{

    public int HitPoints = 10;
    public GameObject[] additionalToDie;
    
    public void TakeDamage(int damageAmount)
    {
        HitPoints -= damageAmount;
        if (HitPoints <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        additionalToDie.ToList().ForEach(x => Destroy(x));
    }
}
