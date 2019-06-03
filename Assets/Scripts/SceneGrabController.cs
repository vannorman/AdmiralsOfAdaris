using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SceneGrabController : MonoBehaviour
{
    public Hand[] hands;
    private bool initialized;
    public Player Player;


    // Start is called before the first frame update

    enum State
    {
        Active,
        Ready
    }
    State state = State.Ready;

    Vector3 posA;
    Vector3 posB;
    float startingDist;
    float startingDistCam;
    Vector3 playerPosStarting;
    Transform tempParent;
    Vector3 startingCenter;
    Vector3 startingLocalCenter;
    Transform fakeCenter;
    float startY;


    Vector3 LocalCenter
    {
        get
        {
            var val = (tempParent.InverseTransformPoint(hands[0].transform.position) + tempParent.InverseTransformPoint(hands[1].transform.position)) / 2f;
            //Utils2.DebugSphere(val);
            fakeCenter.position = val;
            return val;
        }
    }

    Vector3 GlobalCenter
    {
        get
        {
            return (hands[0].transform.position + hands[1].transform.position) / 2f;
        }
    }



    float LocalCurrentDist
    {
        get
        {
            return (tempParent.InverseTransformPoint(hands[0].transform.position) - tempParent.InverseTransformPoint(hands[1].transform.position)).magnitude;
        }
    }

    float GlobalCurrentDist
    {
        get
        {
            return (hands[0].transform.position - hands[1].transform.position).magnitude;
        }
    }

    public bool PlayerGrabbing {
        get
        {
            GrabTypes b = hands[1].GetGrabStarting();
            GrabTypes a = hands[0].GetGrabStarting();
            return (a == GrabTypes.Grip && hands[1].IsGrabbingWithType(GrabTypes.Grip)) ||
                (b == GrabTypes.Grip && hands[0].IsGrabbingWithType(GrabTypes.Grip));
        }
    }

    public bool ReleasedGrab {
        get
        {
            return !hands[0].IsGrabbingWithType(GrabTypes.Grip) || !hands[1].IsGrabbingWithType(GrabTypes.Grip);
        }
    }

    void SetState(State newState)
    {
        state = newState;
        switch (newState)
        {
            case State.Ready:
                if (tempParent) Destroy(tempParent.gameObject);
                Player.transform.SetParent(null);
                Player.enabled = true;
                this.enabled = true;
                break;
            case State.Active:
                tempParent = new GameObject("tempparent").transform;
                fakeCenter = new GameObject("fakecenter").transform;
                fakeCenter.SetParent(tempParent);
                
                tempParent.position = GlobalCenter;
                startingDist = GlobalCurrentDist;
                //startingDistCam = (Player.transform.position - Center).magnitude;
                //playerPosStarting = Player.transform.localPosition;
                Player.transform.SetParent(tempParent);
                startingCenter = GlobalCenter;

                fakeCenter.position = GlobalCenter;
                startingLocalCenter = LocalCenter;
                startY = Quaternion.LookRotation(tempParent.InverseTransformDirection(hands[0].transform.position - hands[1].transform.position), Vector3.up).eulerAngles.y;
                break;

        }
    }

    void Update()
    {
        //if (!initialized)
        //{
        //    Init();
        //    return;
        //}

        switch (state)
        {
            case State.Ready:
                if (PlayerGrabbing)
                {
                     SetState(State.Active);
                }
                break;
            case State.Active:

                // Pan
                var panAmount = -1 * (LocalCenter - startingLocalCenter);
                tempParent.position = startingCenter + panAmount;

                // Zoom
                var zoomAmount = startingDist / LocalCurrentDist;
                var minScale = 2;
                var maxScale = 10;
                //Debug.Log("start:<color=blue>"+startingDist+"</color>, local cur;<color=green>"+LocalCurrentDist+",  </color>lossy:<color=#a50>" + Player.transform.lossyScale.x + ",</color> local:<color=#05a>" + Player.transform.localScale.x+"</color>");
                FindObjectOfType<DebugText>().GetComponent<TextMesh>().text = 
                    "start:" + startingDist 
                    + "\nloc cur:" + LocalCurrentDist 
                    + "\nlossy:" + Player.transform.lossyScale.x 
                    + "\nlocal Play:" + Player.transform.localScale.x 
                    + "\ntempparent:" + tempParent.transform.localScale.x;
                tempParent.transform.localScale = Vector3.one * zoomAmount;
                // Rotation
                var curY = Quaternion.LookRotation(tempParent.InverseTransformDirection(hands[0].transform.position - hands[1].transform.position), Vector3.up).eulerAngles.y;
                tempParent.transform.rotation = Quaternion.Euler(0, startY - curY, 0);
          

                if (ReleasedGrab)
                {
                    SetState(State.Ready);
                }
                break;

        }


        

    }

    private void Init()
    {
        //hands = FindObjectsOfType<Hand>();
        //if (hands.Length == 2)
        //{
        //    initialized = true;
        //    Debug.Log("<color=blue>Initialized</color>");
        //}
    }
}
