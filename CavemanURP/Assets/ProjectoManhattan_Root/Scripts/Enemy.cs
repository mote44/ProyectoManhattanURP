using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int i; //indice del array de puntos para que el enemigo detecte un punto a seguir.
    [Header("Point & Movement Configuration")]
    [SerializeField] Transform[] points; //Array de puntos de posición hacia los que el enemigo se moverá.
    [SerializeField] int startingPoint; //Número mpara determinar el punto de inicio del enemigo.
    [SerializeField] float speed; //Velocidad de la plataforma

    [Header("Enemy References")]
    [SerializeField] private float enemyLife;

    Rigidbody2D enemyRb;
    private Animator enemyAnim;


    [SerializeField] bool isGrounded;
    [SerializeField] GameObject groundCheck;
    [SerializeField] LayerMask groundLayer; //Sirve para decirle al personaje cuál es la capa suelo
    [SerializeField] Vector2 groundCheckSize;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[startingPoint].position;

        enemyAnim = GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody2D>();
        groundCheck = GameObject.Find("EnemyGroundCheck");//Encuentra el object que hemos creado como hijo de Player
        groundCheckSize = new Vector2(.8f, .04f);

    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.transform.position, groundCheckSize, 0f, groundLayer);

        if (enemyLife > 0)
        {
            if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
            {
                transform.localScale = new Vector3(1, 1, 1);
                enemyAnim.SetBool("Walk", true);
                i++; //Aumenta el indice, cambia de objetivo hacia el que moverse.
                if (i == points.Length) //Chequea si el enemigo ha llegado al ultimo punto del array.
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    i = 0;//Resetea el índice para volver a empezar, el enemigo va hacia el punto 0.
                }
            }


            //Mueve el enemigo a la posición del punto guardado en el array...
            //... que corresponda al espacio del array con valor igual a la variable "i"
            transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        }

       
    }

    private void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if (enemyLife > 0)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                enemyAnim.SetTrigger("Attack");
            }
        }
            
    }

    public void DamageReceive(float damage)
    {
        enemyLife -= damage;

        if (enemyLife <= 0) { EnemyDeath(); }
    }

    private void EnemyDeath()
    {
        StartCoroutine(Muerte());
        
    }


    IEnumerator Muerte()
    {
        enemyAnim.SetTrigger("Death");
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);

    }
}
