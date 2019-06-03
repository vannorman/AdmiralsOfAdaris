using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageGiver : MonoBehaviour
{
    public int damageAmout = 1;
    private void OnCollisionEnter(Collision collision)
    {
        collision.collider.GetComponent<DamageReceiver>()?.TakeDamage(damageAmout);
        Destroy(this.gameObject);
        Debug.Log("hit:" + collision.collider.name);
    }
}
