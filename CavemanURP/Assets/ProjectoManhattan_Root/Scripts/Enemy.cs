using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int i; //indice del array de puntos para que el enemigo detecte un punto a seguir.
    [Header("Point & Movement Configuration")]
    [SerializeField] Transform[] points; //Array de puntos de posici�n hacia los que el enemigo se mover�.
    [SerializeField] int startingPoint; //N�mero mpara determinar el punto de inicio del enemigo.
    [SerializeField] float speed; //Velocidad de la plataforma

    [Header("Enemy References")]
    [SerializeField] private float enemyLife;


    AudioSource audioSour;
    Rigidbody2D enemyRb;
    private Animator enemyAnim;


    [SerializeField] bool isGrounded;
    [SerializeField] GameObject groundCheck;
    [SerializeField] LayerMask groundLayer; //Sirve para decirle al personaje cu�l es la capa suelo
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] GameObject enemyHit;
    [SerializeField] GameObject enemyLight;
    [SerializeField] AudioSource enemySound;

    private void Awake()
    {
        enemyHit = GameObject.Find("EnemyHit"); //Encuentra el object con el collider de ataque
        enemyLight = GameObject.Find("EnemyLight");
        enemySound = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.position = points[startingPoint].position;

        enemyAnim = GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody2D>();
        audioSour = GetComponent<AudioSource>();
        groundCheck = GameObject.Find("EnemyGroundCheck");//Encuentra el object que hemos creado como hijo de Player
        groundCheckSize = new Vector2(.8f, .04f);
        enemyHit.SetActive(false);

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
                    i = 0;//Resetea el �ndice para volver a empezar, el enemigo va hacia el punto 0.
                }
            }


            //Mueve el enemigo a la posici�n del punto guardado en el array...
            //... que corresponda al espacio del array con valor igual a la variable "i"
            transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        }

       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enemyLife > 0)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                enemyAnim.SetTrigger("Attack");
                StartCoroutine(IsAttacking());
            }
        }
            
    }

    public void DamageReceive(float damage)
    {
        enemyLife -= damage;

        if (enemyLife <= 0) { StartCoroutine(EnemyDeath()); }
    }

    private IEnumerator EnemyDeath()
    {
        yield return new WaitForSeconds(0.2f);
        enemyAnim.SetTrigger("Death");
        enemyLight.SetActive(false);
        audioSour.Pause();
        enemySound.Stop();
        //audioSour.mute = true;
        yield return new WaitForSeconds(0.7f);
        gameObject.SetActive(false);
        yield return null;
        
    }

    IEnumerator IsAttacking()
    {
        
        yield return new WaitForSeconds(0.2f);
        enemyHit.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        enemyHit.SetActive(false);
        yield return null;
    }

}
