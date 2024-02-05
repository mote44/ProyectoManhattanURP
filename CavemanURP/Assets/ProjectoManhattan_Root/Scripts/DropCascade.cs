using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCascade : MonoBehaviour
{

    [SerializeField] GameObject dropPrefab;
    [SerializeField] GameObject shootPoint;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(dropPrefab, shootPoint.transform.position, Quaternion.identity);
    }

   

   
}
