using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDirectionImMoving : MonoBehaviour {

    RecordPosition rp;
	// Use this for initialization
	void Start () {
		rp = GetComponent<RecordPosition>();

	}
    float t = 0.05f;
    bool active = false;
	// Update is called once per frame
	void Update () {
        if (!active)
        {
            t -= Time.deltaTime;
            if (t < 0)
                active = true;
            return;
        }
        if (Mathf.Abs((rp.nowPosition - rp.lastPosition).magnitude) > .01f)
        {
            transform.forward = rp.nowPosition - rp.lastPosition;
        }     
		
	}
}
