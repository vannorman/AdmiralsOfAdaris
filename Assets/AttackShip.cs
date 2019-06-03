using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
namespace AoA

{
    public class AttackShip : MonoBehaviour
{
	public Transform firePos;
	public GameObject bulletPrefab;

	public float attackRange = 1.5f;


	public float interval = 1.5f;
	float t = 0;
	void Update()
	{
		t -= Time.deltaTime;
		if (t < 0)
		{
			//var interval = Random.Range(0.2f,0.21f);
			t = interval;
			FindAndAttackEnemy();
		}
	}

    void FindAndAttackEnemy()
    {
        //Debug.Log("ship " + name + " is checking ..");
        if (currentTarget != null && TargetOutOfRange())
        {
            currentTarget = null;
        }
        if (!currentTarget)
        {
            currentTarget = TryAcquireTarget();
        }
        if (currentTarget)
        {
            GetComponent<ShipMotor>().SetState(ShipMotor.State.Attacking);
            TryFireAtTarget();
        }
    }

    private void TryFireAtTarget()
    {
        var attackAngle = 30;
        var dir = currentTarget.position - transform.position;
        if (Vector3.Angle(transform.forward, dir) < attackAngle)
        {
            FireAtEnemy(dir);
        }
    }

    private bool TargetOutOfRange()
    {
        return Vector3.Magnitude(currentTarget.position - transform.position) > attackRange;
    }

    private Transform TryAcquireTarget()
    {
       
        float min = Mathf.Infinity;
        DamageReceiver closestEnemy = null;

        foreach (var dr in FindObjectsOfType<DamageReceiver>())
        {
            if (dr.GetComponent<TeamIdentifier>().color != this.GetComponent<TeamIdentifier>().color)
            {
                var dir = dr.transform.position - transform.position;


                if (dir.magnitude < attackRange)
                {
                    currentTarget = dr.transform;
                    if (dir.magnitude < min)
                    {
                        min = dir.magnitude;
                        closestEnemy = dr;
                    }
                }
            }
        }
        return closestEnemy ? closestEnemy.transform : null;
    }
    

    public Transform currentTarget;
	public int bulletForce = 30;
	private void FireAtEnemy(Vector3 dir)
	{
		Utils2.MakeSound(fireSound, transform.position);
        var bullet = PhotonNetwork.Instantiate("PUNBullet", firePos.position, firePos.rotation, 0);
        //var bullet = (GameObject)Instantiate(bulletPrefab, firePos.position, firePos.rotation);
		bullet.GetComponent<Rigidbody>().AddForce(dir.normalized * bulletForce);
	}

	
	public AudioClip fireSound;
    //private float cooldown;

    internal void AttackCooldown(int v)
    {
        t = v;
    }
}

}
