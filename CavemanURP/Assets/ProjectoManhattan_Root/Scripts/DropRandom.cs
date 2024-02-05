using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRandom : MonoBehaviour
{
    Animator animat;
    Rigidbody2D dropRb;

    [SerializeField] LayerMask groundLayers;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] GameObject groundCheckDrop;
    [SerializeField] private float groundCheckRadiuss;


    private Vector2 initialPos;


    [SerializeField] private bool isGround;
    [SerializeField] private bool isPlayer;
    private float dropInitialGravity;
    // Start is called before the first frame update
    private void Awake()
    {

        dropRb = GetComponent<Rigidbody2D>();
        animat = GetComponent<Animator>();
        groundCheckDrop = GameObject.Find("groundCheckDrop1");//Encuentra el object que hemos creado como hijo de Player
        groundCheckRadiuss = .25f;
        dropInitialGravity = dropRb.gravityScale;
        initialPos = transform.position;


    }

    void Start()
    {

        isGround = false;
        dropRb.bodyType = RigidbodyType2D.Dynamic;


    }




    void Update()
    {


        isGround = Physics2D.OverlapCircle(groundCheckDrop.transform.position, groundCheckRadiuss, groundLayers);
        isPlayer = Physics2D.OverlapCircle(groundCheckDrop.transform.position, groundCheckRadiuss, playerLayer);
        if (isPlayer || isGround)
        {
            StartCoroutine(Drop());
        }

    }


    IEnumerator Drop()
    {

        animat.SetBool("Drop", false);
        animat.SetBool("Colision", true);
        yield return new WaitForSeconds(1);

        animat.SetBool("Colision", false);
        animat.SetBool("Drop", true);
        transform.position = initialPos;

    }

    
}

