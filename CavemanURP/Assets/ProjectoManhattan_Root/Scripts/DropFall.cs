using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DropFall : MonoBehaviour
{
    Animator animator;
    Rigidbody2D dropRb;
    BoxCollider2D col;
    [SerializeField]Light2D dropLight;

    [SerializeField] LayerMask groundLayers;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] GameObject groundCheckDrop;
    [SerializeField] private float groundCheckRadiuss;
    private Vector2 raycastPos1;
    private Vector2 raycastPos2;
    
    private Vector2 initialPos;


    [SerializeField] private bool isGround;
    [SerializeField] private bool isPlayer;
    private float dropInitialGravity;
    // Start is called before the first frame update
    private void Awake()
    {
        dropRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundCheckDrop = GameObject.Find("groundCheckDrop");//Encuentra el object que hemos creado como hijo de Player
        groundCheckRadiuss = .25f;
        dropInitialGravity = dropRb.gravityScale;
        initialPos = transform.position;
        col = GetComponent<BoxCollider2D>();

    }

    void Start()
    {
        
        isGround = false;
        dropRb.bodyType = RigidbodyType2D.Static;
        
        
        raycastPos1 = new Vector2(transform.position.x + 3f, transform.position.y);
        raycastPos2 = new Vector2(transform.position.x - 3f, transform.position.y);
       


    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //RaycastHit2D hit = Physics2D.Raycast(raycastPos1, -Vector2.up, Mathf.Infinity, playerLayer);
        //RaycastHit2D hit2 = Physics2D.Raycast(raycastPos2, -Vector2.up, Mathf.Infinity, playerLayer);
        /*if (hit.collider.gameObject.CompareTag("Player"))  //|| hit2.collider.CompareTag("Player") )
        {
            dropRb.gravityScale = dropInitialGravity;
            dropRb.bodyType = RigidbodyType2D.Dynamic;
        }*/
      


    }

    void Update()
    {
        isGround = Physics2D.OverlapCircle(groundCheckDrop.transform.position, groundCheckRadiuss, groundLayers);
        isPlayer = Physics2D.OverlapCircle(groundCheckDrop.transform.position, groundCheckRadiuss, playerLayer);
        if ( isPlayer || isGround )
        {
            StartCoroutine(Drop());
        }
        
    }


    IEnumerator Drop()
    {
        animator.SetBool("Drop", false);
        animator.SetBool("Colision",true);
        yield return new WaitForSeconds(.1f);
        dropLight.intensity = 0;
        col.enabled = false;
        dropRb.bodyType = RigidbodyType2D.Static;
        animator.SetBool("Colision", false);
        animator.SetBool("Drop", true);
        transform.position = initialPos;
        col.enabled = true;
        dropLight.intensity = 1;


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            dropRb.gravityScale = dropInitialGravity;
            dropRb.bodyType = RigidbodyType2D.Dynamic;

        }
    }
    void OnDrawGizmos()
    {
        //Gizmos.DrawRay(raycastPos1, -Vector2.up);
        //Gizmos.DrawRay(raycastPos2, -Vector2.up);
    }
}
