using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class CheckpointController : MonoBehaviour
{
    AudioSource audioS;
    Animator anim;
    [SerializeField] Light2D checkpoinLight;
    
    [SerializeField] private Vector2 pointPos;


  

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioS = GetComponent<AudioSource>();
       
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("onFire", true);
            pointPos = this.transform.position;
            checkpoinLight.enabled = true;
            audioS.Play();
            

        }
    }
}
