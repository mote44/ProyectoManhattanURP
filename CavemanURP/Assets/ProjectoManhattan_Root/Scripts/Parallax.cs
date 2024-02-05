using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private float parallaxMultiplier;
    

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;
    void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;

    }

    // Update is called once per frame
    void LateUpdate()
    {
        float deltaX =(cameraTransform.position.x - previousCameraPosition.x) * parallaxMultiplier;
        transform.Translate(new Vector3(deltaX,0,0));
        previousCameraPosition = cameraTransform.position;
    }
}
