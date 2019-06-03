using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometSpawner : MonoBehaviour
{
    public float spawnInterval = 5;

    float t = 0;
    public GameObject cometPrefab;
    public float speed = 1;
    public bool rotate = false;
    public float rotspeed = 4;
    public float minSize = 1;
    public float maxSize = 1;
    public float spawnRange = 0;
    void Update()
    {
        t -= Time.deltaTime;
        if (t < 0)
        {
            t = spawnInterval;
            GameObject comet = (GameObject)Instantiate(cometPrefab, transform.position + spawnRange * Random.insideUnitSphere, transform.rotation);
            var ma = comet.AddComponent<MoveAlongAxis>();
            ma.speed = speed;
            ma.transform.localScale = Vector3.one * Random.Range(minSize, maxSize);
            if (rotate)
            {
                ma.space = Space.World;
                ma.dir = transform.forward;
                var rot = comet.AddComponent<Rotate>();
                rot.dir = Random.insideUnitSphere;
                rot.rotSpeed = rotspeed;
            }
            else
            {
                ma.space = Space.Self;
                ma.dir = Vector3.forward;
            }
        }
    }
}
