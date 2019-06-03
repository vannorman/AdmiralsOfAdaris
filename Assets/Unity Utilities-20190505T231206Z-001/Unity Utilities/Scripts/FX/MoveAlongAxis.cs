using UnityEngine;
using System.Collections;

public class MoveAlongAxis : MonoBehaviour {

    // a simple "move" script that is used for moving along Sine waves and possibly platforms.
    public Space space;
	public Vector3 dir;
	public float speed;

    private void Start()
    {
        if (!GetComponent<Rigidbody>())
        {
            var rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;

        }
    }

    void Update () {
        //			// commented Debug.Log("tt:"+tt);
        //transform.Translate(dir * speed * Time.deltaTime, space);
        var realDir = space == Space.World ? dir : transform.TransformDirection(dir);
        GetComponent<Rigidbody>().AddForce(dir * speed * 100 * Time.deltaTime);
	}
	
	
}
