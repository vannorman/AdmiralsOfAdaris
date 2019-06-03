using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AoA
{

    public class ShipMotor : MonoBehaviour
    {
	    public float moveSpeed = 10;
	    public float rotSpeed = 50;
	    public float rbMoveSpeed = 1f;
	    //public float rbRotSpeed = 0.7f;
	    public Transform ghostShip;
	    public enum State
	    {
		    Idle,
		    MoveFromFar,
		    MoveWhileNear,
            Attacking,
            PlayerMoving
        }
	    State state;
	    public AudioClip idleSound;
	    public AudioClip movingSound;
	    public float farMovementBehaviorThreshold = 0.2f;
	    private Rigidbody rb;

	    private void Start()
	    {
		    rb = GetComponent<Rigidbody>();
            SetState(State.Idle);
	
	    }

	    void Update()
	    {
            var targetRotation = transform.rotation;
		    float ghostDelta = Vector3.Magnitude(ghostShip.position - transform.position);
		    var ghostAngle = Vector3.Angle(ghostShip.forward, transform.forward);

		    switch (state)
		    {
			    case State.Idle:
				    break;
			    case State.MoveFromFar:

				    if (ghostDelta > farMovementBehaviorThreshold)
				    {
					    //var minSpeed = 1f / (ghostShip.position - transform.position).magnitude;
					    rb.AddForce(transform.forward * Time.deltaTime * rbMoveSpeed);

					    //get the angle between transform.forward and target delta
					    //var dir = (ghostShip.transform.position - transform.position).normalized;
					    //float angleDiff = Vector3.Angle(transform.forward, dir);

					    //// get its cross product, which is the axis of rotation to
					    //// get from one vector to the other
					    //Vector3 cross = Vector3.Cross(transform.forward, dir);

					    //// apply torque along that axis according to the magnitude of the angle.
					    //rb.AddTorque(cross.normalized * angleDiff * rbRotSpeed);
					    targetRotation = Quaternion.LookRotation((ghostShip.position - transform.position).normalized);
					
				    }
				    else
				    {
					    SetState(State.MoveWhileNear);
				    }
				    break;
			    case State.MoveWhileNear:
				    transform.position = Vector3.MoveTowards(transform.position, ghostShip.position, Time.deltaTime * moveSpeed);
				    targetRotation = Quaternion.RotateTowards(transform.rotation, ghostShip.rotation, Time.deltaTime * rotSpeed);
				    if (ghostDelta < .1f && ghostAngle < .1f)
				    {
					    SetState(State.Idle);
				    }
                    if (ghostDelta > farMovementBehaviorThreshold)
                    {
                        SetState(State.MoveFromFar);
                    }
				    break;
                case State.Attacking:
                    targetRotation = Quaternion.LookRotation((GetComponent<AttackShip>().currentTarget.position - transform.position).normalized);
                    break;
                case State.PlayerMoving:
                    break;
		    }

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotSpeed);

            if (Input.GetKeyDown(KeyCode.G))
		    {
			    SetState(State.MoveFromFar);
		    }

	    }

	    public void SetState(State newState)
	    {
		    Debug.Log("<color=blue><size=15>ShipState:</size></color>" + newState);
		    state = newState;
		    switch (state)
		    {
			    case State.Idle:
				    GetComponent<AudioSource>().clip = idleSound;
                    GetComponent<AttackShip>().AttackCooldown(0);

                    GetComponent<AudioSource>().Play();
                    GetComponent<Rigidbody>().isKinematic = true;
                    //ghostShip.gameObject.SetActive(false);
                    break;
                case State.MoveWhileNear:
                    GetComponent<AttackShip>().AttackCooldown(0);
                    GetComponent<Rigidbody>().isKinematic = true;
                    break;
			    case State.MoveFromFar:
                    GetComponent<Rigidbody>().isKinematic = false;
                    GetComponent<AttackShip>().AttackCooldown(3);
                    GetComponent<AudioSource>().clip = movingSound;
				    GetComponent<AudioSource>().Play();
			    break;
		    }
	    }



    }
}
