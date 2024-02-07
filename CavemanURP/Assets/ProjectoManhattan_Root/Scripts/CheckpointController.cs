using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckpointController : MonoBehaviour
{
    
    Animator anim;
    
    [SerializeField] private Vector2 pointPos;


  

    private void Awake()
    {
        anim = GetComponent<Animator>();
       
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("onFire", true);
            pointPos = this.transform.position;
            
            
        }
    }
}
