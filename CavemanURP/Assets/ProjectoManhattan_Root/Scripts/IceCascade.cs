using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCascade : MonoBehaviour
{
    [SerializeField] GameObject icePrefab;
    [SerializeField] GameObject shootPoint;
    private int randomNum;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(icePrefab, shootPoint.transform.position, Quaternion.identity);
    }




}

