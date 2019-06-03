using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activeAfterTime : MonoBehaviour
{
    public float waitTime = .15f;
    public float DestroyTime = 1.0f;
    public GameObject This;
    public GameObject AdditionalEffects;
    float StartTime;
    float GoTime;
    bool Activated = false;
    // Start is called before the first frame update
    void Start()
    {
        StartTime = Time.time;
        AdditionalEffects.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Activated == false)
        {
            if (StartTime + waitTime <= Time.time)
            {
                AdditionalEffects.SetActive(true);
                Activated = false;
            } 
        }
        if (StartTime + DestroyTime <= Time.time)
        {
            Destroy(This);
        }
    }
}
