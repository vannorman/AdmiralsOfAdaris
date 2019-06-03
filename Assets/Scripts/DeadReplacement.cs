using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadReplacement : MonoBehaviour
{


    public GameObject replacementPrefab;
    private void OnDestroy()
    {
        Instantiate(replacementPrefab, transform.position, transform.rotation);
    }
}
