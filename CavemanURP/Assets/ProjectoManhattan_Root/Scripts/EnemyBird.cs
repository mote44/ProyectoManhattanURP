using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBird : MonoBehaviour
{
    [Header("Bird References")]
    [SerializeField] private float birdLife;

    Rigidbody2D birdRb;
    private Animator birdAnim;
    [SerializeField] LayerMask groundLayer; //Sirve para decirle al personaje cuál es la capa suelo

    [Header("Chase")]
    public Transform playerTransform;
    [SerializeField] float speed;
    [SerializeField] bool isChasing;
    [SerializeField] float chaseDistance;
    [SerializeField] GameObject birdLight;
    Vector2 chaseAim;


    private void Awake()
    {
        birdLight = GameObject.Find("BirdLight");
    }


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
            StopCoroutine(PlayerDetected());
            StartCoroutine(Chase());
        }
        else
        {
            if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
            {
                chaseAim = playerTransform.position;
                StartCoroutine(PlayerDetected());
            }
        }

        //condiciones para flipear el personaje segun movimiento


        if (isChasing && playerTransform.position.x - transform.position.x < 0)
         {
         transform.localScale = new Vector3(1, 1, 1);
         }
         if (isChasing && playerTransform.position.x - transform.position.x > 0)
         {
         transform.localScale = new Vector3(-1, 1, 1);
         }
    }



    public void DamageReceive(float damage)
    {
        birdLife -= damage;

        if (birdLife <= 0) { StartCoroutine(EnemyDeath()); }
    }

    private IEnumerator EnemyDeath()
    {
        yield return new WaitForSeconds(0.2f);
        birdAnim.SetTrigger("Enemy_2_Death");
        birdLight.SetActive(false);
        yield return new WaitForSeconds(0.7f);
        gameObject.SetActive(false);
        yield return null;
    }

    private IEnumerator PlayerDetected()
    {
        yield return new WaitForSeconds(0.2f);
        isChasing = true;
        yield return new WaitForSeconds(1);
        isChasing = false;
        yield return null;
    }

    private IEnumerator Chase()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = Vector2.MoveTowards(transform.position, chaseAim, speed * Time.deltaTime);
        yield return new WaitForSeconds(2);
        //isChasing = false;
        yield return null;
    }

}
