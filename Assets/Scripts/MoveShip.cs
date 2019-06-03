using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
namespace AoA
{
    public class MoveShip : MonoBehaviour
    {
        public GameObject ghostShip;

	    public ShipMotor ShipMotor;
	    public AudioClip grabSound;
	    public AudioClip letGoSound;

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Interactable>().onAttachedToHand += Grabbed;
            GetComponent<Interactable>().onDetachedFromHand += LetGo;
        }

        private void LetGo(Hand hand)
        {
		    Utils2.MakeSound(letGoSound, transform.position);
		    ShipMotor.SetState(ShipMotor.State.MoveFromFar);
		    Debug.Log("Grabbed at:" + transform.position);
            Utils2.DebugSphere(transform.position).GetComponent<Renderer>().material.color = Color.blue;
            // Place ghost instance 
            // Instruct real instance to navigate to this location
        }

        private void Grabbed(Hand hand)
        {
		    Utils2.MakeSound(grabSound, transform.position);

             ShipMotor.SetState(ShipMotor.State.PlayerMoving);
            Utils2.DebugSphere(transform.position).GetComponent<Renderer>().material.color = Color.red;
            ghostShip.SetActive(true);
            Debug.Log("Let go at:" + transform.position);
            // Create ghost instance
            // Leave real instance where it is
        }

    
    }

}
