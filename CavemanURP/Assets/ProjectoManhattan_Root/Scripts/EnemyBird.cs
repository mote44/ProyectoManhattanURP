using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBird : MonoBehaviour
{
    [Header("Bird References")]
    [SerializeField] private float birdLife;

    Rigidbody2D birdRb;
    private Animator birdAnim;

    [Header("Chase")]
    float horizontalMovement;
    public Transform playerTransform;
    [SerializeField] float speed;
    [SerializeField] bool isChasing;
    [SerializeField] float chaseDistance;





    // Start is called before the first frame update
    void Start()
    {
        birdAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChasing)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
        }
        else
        {
            if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
            {
                isChasing = true;
            }
        }

        //condiciones para flipear el personaje segun movimiento


        //if (isChasing && )
        // {
        // transform.localScale = new Vector3(1, 1, 1);
        // }
        // if (isChasing && )
        // {
        // transform.localScale = new Vector3(-1, 1, 1);
        // }
    }



    public void DamageReceive(float damage)
    {
        birdLife -= damage;

        if (birdLife <= 0) { EnemyDeath(); }
    }

    private void EnemyDeath()
    {
        birdAnim.SetTrigger("Death_Enemy_2");
    }



}
