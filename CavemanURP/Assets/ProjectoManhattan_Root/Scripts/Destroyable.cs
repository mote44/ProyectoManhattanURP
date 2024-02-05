using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    // public GameObject objetoRoto; 

    Collider2D col;
    Animator animator;
    public GameObject jugador;
    
    public float playerSpeed;

    [Header("Break Direction")]
    public bool forwardImpact;
    public float speedToBreak = .9f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        playerSpeed =  jugador.GetComponent<Rigidbody2D>().velocity.x;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (forwardImpact)
        {
            if (collision.gameObject.tag == "Player" && playerSpeed > speedToBreak)
            {
               
                Shake.Instance.CameraMovement(5f,5f,0.5f);
                //Instantiate(objetoRoto, transform.position, Quaternion.identity);
                //Destroy(this.gameObject);
                animator.SetBool("colision", true);
                col.enabled = false;
                playerSpeed = playerSpeed * -1;
                jugador.GetComponent<Rigidbody2D>().AddForce(Vector2.right*-2, ForceMode2D.Impulse); //Impulso hacia abajo
                



            }
        }
        else
        {
            if (collision.gameObject.tag == "Player" && playerSpeed < -speedToBreak)
            {
                Shake.Instance.CameraMovement(5f, 5f, 0.5f);
                //Instantiate(objetoRoto, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
                playerSpeed = playerSpeed * -1;
                jugador.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 2, ForceMode2D.Impulse);
            }
        }
        
    }
}


//collision.rigidbody.velocity.x > impactSpeed