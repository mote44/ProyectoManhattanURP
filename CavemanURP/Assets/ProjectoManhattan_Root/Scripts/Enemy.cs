using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float enemyLife;

    Rigidbody2D enemyRb;
    private Animator enemyAnim;


    [SerializeField] bool isGrounded;
    [SerializeField] GameObject groundCheck;//Un objeto que detecta el suelo
    [SerializeField] LayerMask groundLayer; //Sirve para decirle al personaje cuál es la capa suelo
    [SerializeField] Vector2 groundCheckSize;

    // Start is called before the first frame update
    void Start()
    {
       enemyAnim = GetComponent<Animator>();
       enemyRb = GetComponent<Rigidbody2D>();
       groundCheck = GameObject.Find("EnemyGroundCheck");//Encuentra el object que hemos creado como hijo de Player
       groundCheckSize = new Vector2(.8f, .04f);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.transform.position, groundCheckSize, 0f, groundLayer);
    }

    public void DamageReceive(float damage)
    {
        enemyLife -= damage;

        if (enemyLife <= 0) { EnemyDeath(); }
    }

    private void EnemyDeath()
    {
        
    }
}
