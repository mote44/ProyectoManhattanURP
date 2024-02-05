using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Shake : MonoBehaviour
{
    private CinemachineVirtualCamera vCamera;
    private CinemachineBasicMultiChannelPerlin mPerlin;
    private float movTime;
    private float totalMovTime;
    private float initialIntensity;

    public static Shake Instance;


    private void Awake()
    {
        Instance = this;
        vCamera = GetComponent<CinemachineVirtualCamera>();
        mPerlin = vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void CameraMovement(float intensity, float frequence, float time)
    {
        mPerlin.m_AmplitudeGain = intensity;
        mPerlin.m_FrequencyGain = frequence;
        initialIntensity = intensity;
        movTime = time;
        totalMovTime = time;
    }

    private void Update()
    {
        if(movTime > 0)
        {
            movTime -= Time.deltaTime;
            //con Lerp vamos del primer valor al segundo en el tiempo que dura el tercero
            mPerlin.m_AmplitudeGain = Mathf.Lerp(initialIntensity, 0 , 1-(movTime/totalMovTime)); 
        }
    }
}

