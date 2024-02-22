using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerControllerX : MonoBehaviour
{
    //Variables de referencia
    [SerializeField] AudioClip audioClip;
    AudioSource audioSour;
    Rigidbody2D playerRb;
    Animator anim;
    [SerializeField] Light2D torch;
    [SerializeField] private ParticleSystem snowParticles;
    float horizontalInput;

    [Header("Movement & Jump")]
    public float speed;
    public float jumpForce;
    public float dashForce;
    bool isFacingRight = true;
    

    public int lifeCounter;
    public int superLifeCounter;
    bool stop = false;
    private Vector3 respawnPos;
    [SerializeField] bool isGrounded;
    [SerializeField] bool isOnPlatform;
    [SerializeField] bool isOnWall;
    [SerializeField] bool isOnWater;
    [SerializeField] GameObject groundCheck;//Un objeto que detecta el suelo
    [SerializeField] GameObject platformCheck;
    [SerializeField] GameObject wallCheck;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] Vector2 wallCheckSize;

    [SerializeField] LayerMask groundLayer; //Sirve para decirle al personaje cuál es la capa suelo
    [SerializeField] LayerMask platformLayer;
    
    
   

    [Header("Hit")]
    [SerializeField] Transform hitCheck;
    [SerializeField] private float hitCheckRadius = 1f; //Define el radio de 
    [SerializeField] private float hitDamage;
    [SerializeField] private bool isHitting;
    private Vector2 torchInitPos;
    [SerializeField] private int damageReceived;

    [Header("Dash")]
    [SerializeField] private bool canDash;
    [SerializeField] private bool isDashing = false;
    [SerializeField] private float dashingPower = 2f;
    [SerializeField] private float dashingTime = .1f;
    [SerializeField] private float dashingCooldown = 1f;
    //[SerializeField] private TrailRenderer tr;
    [SerializeField] GameObject trailObject;
    

    [Header("SaltoRegulable")]
    [Range(0,1)] [SerializeField] private float multiplicadorSalto;
    [SerializeField] private float multiplicadorGravedad;
    private float playerGravityScale;
    
    



    // Start is called before the first frame update

    private void Awake() //Va antes de start
    {
        playerRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSour = GetComponent<AudioSource>();
        groundCheck = GameObject.Find("GroundCheck");//Encuentra el object que hemos creado como hijo de Player
        groundCheckSize = new Vector2(.78f, .04f);
        wallCheckSize = new Vector2(.85f, .2f);
        damageReceived = 1;
        superLifeCounter = 3;
        
        //AudioManager.Instance.PlaySFX(0);     //Lanzar audios



    }

    void Start()
    {
        canDash = true;
        playerGravityScale = playerRb.gravityScale;
        respawnPos = transform.position;
        
    }


    private void FixedUpdate() //Creado para cancelación de salto
    {
      
        
        if (playerRb.velocity.y < 0 && !isGrounded)                             //Si desciende pero no está en suelo
        {
            playerRb.gravityScale = playerGravityScale * multiplicadorGravedad; //La gravedad del RB = variable GravEscalaInicial * multiplicador (Mayor que 1 para acelerar)
        }
        else
        {
            playerRb.gravityScale = playerGravityScale;                         //Si no, se resetea la escala del RB
        }
                                                               
    }

    // Update is called once per frame
    void Update()
    {
        
        torchInitPos = new Vector2(transform.position.x - .5f,transform.position.y + 1.1f);

        if (isDashing)
        {
            return;
        }
        //Disparar el Dash
        if (Input.GetKeyDown(KeyCode.LeftAlt) && canDash && (playerRb.velocity.x > 1.2f || playerRb.velocity.x < -1.2f))
        {
            anim.SetTrigger("Dash");
            StartCoroutine(Dash());
        }

        isGrounded = Physics2D.OverlapBox(groundCheck.transform.position, groundCheckSize, 0f, groundLayer);
        isOnPlatform = Physics2D.OverlapBox(platformCheck.transform.position, groundCheckSize, 0f, platformLayer);
        
        //if (!isGrounded) { isOnPlatform = false; }
        
        //isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, groundCheckRadius, groundLayer);   //Physics2D dibuja figuras geometricas invisibles
        //isGrounded va a ser verdadero     //Desde dónde?                   ,Radio?           , qué capa va a detectar? 
        isOnWall = Physics2D.OverlapBox(wallCheck.transform.position, wallCheckSize, 0f, groundLayer); ;  //Physics2D dibuja figuras geometricas invisibles
        
        anim.SetBool("Jump", (!isGrounded && !isOnPlatform));
        anim.SetBool("Run", (isGrounded || isOnPlatform) && horizontalInput!=0);
        anim.SetInteger("Life", lifeCounter);

        Movement(); // LAS FÍSICAS DEBERÍAN IR EN EL FIXED UPDATE
        Jump();
        Hit();
        AirHit();
        

        if (lifeCounter <= 0)
        {
            PlayerDead();


        }

        
       
        
        if (horizontalInput > 0) //Si 
        {
            if (!isFacingRight)
            {
                Flip();
            }
        }
        if (horizontalInput < 0)
        {
            if (isFacingRight)

            {
                Flip();
            }
        } 
        
        
    }

    

    void Movement()
    {
        //Referenciar el INPUT
        horizontalInput = Input.GetAxis("Horizontal"); // Rellenamos la variable del input horizontal
        playerRb.velocity = new Vector2(horizontalInput * speed / playerRb.gravityScale, playerRb.velocity.y); //El valor Y representa el valor que tenga la posY del player en cada momento, no queremos modificarla

        if ((playerRb.velocity.x > 0.1f || playerRb.velocity.x < -0.1f) && isGrounded)
        { 
            audioSour.mute = false;
            
        }
        else
        {
            audioSour.mute = true;
            //audioSour.PlayOneShot(audioClip);
        }
        
       
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || isOnPlatform)) 
        {
            playerRb.mass = 1f;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse); //Definimos el tipo de fuerza (Impulse para salto) después de definir que el salto es moverse en vertical * jumpforce
            if(!isOnPlatform)snowParticles.Play();
            AudioManager.Instance.PlaySFX(1);
            AudioManager.Instance.PlaySFX(9);

        }

        if (Input.GetKeyDown(KeyCode.Space) && isOnWater)
        {
            playerRb.mass = 1.5f;
            playerRb.AddForce(Vector3.up * (jumpForce/1.5f), ForceMode2D.Impulse); //Definimos el tipo de fuerza (Impulse para salto) después de definir que el salto es moverse en vertical * jumpforce
            
        }

        //Cancelación salto
        if (Input.GetKeyUp(KeyCode.Space))    //Si se suelta el botón salto  
        {
            JumpKeyUp();                    //Ejecuta 
        }

        

    }

    private void JumpKeyUp()
    {
        if (playerRb.velocity.y > 0) //Si está subiendo
        {
            playerRb.AddForce(Vector2.down * playerRb.velocity.y * (1 - multiplicadorSalto), ForceMode2D.Impulse); //Impulso hacia abajo
        }


    }


    void Flip()
    {
        Vector3 currentScale = transform.localScale; //Esta variable es igual a nuestra escala actual
        currentScale.x *= -1; //Invierte el X de currentScale
        transform.localScale = currentScale;
        isFacingRight = !isFacingRight; //Invierte el valor del booleano isFacingRight
        
        if (isGrounded)
        {
            snowParticles.Play();
        }
    }



    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        
        float originalGravity = playerRb.gravityScale;
        playerRb.gravityScale = 0.5f;
        playerRb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        AudioManager.Instance.PlaySFX(3);
        AudioManager.Instance.PlaySFX(10);
        //tr.emitting = true;
        trailObject.GetComponent<TrailRenderer>().emitting = true;
        yield return new WaitForSeconds(dashingTime);
        //tr.emitting = false;
        trailObject.GetComponent<TrailRenderer>().emitting = false;
        playerRb. gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }


    void Respawn()
    {
        transform.position = respawnPos;
        lifeCounter = 3;
        anim.SetInteger("Life", 3);
        torch.intensity = 1f;
        stop = false;
    }
    
    void PlayerDead()
    {


        if (!stop)
        {
            StartCoroutine(Death());
            superLifeCounter -= 1;
            GameManager.Instance.LifeDown(superLifeCounter);
            stop = true;
        }


    }
    private IEnumerator Death()
    {
        
        anim.SetInteger("Life", 0);
        speed = 0f;
        yield return new WaitForSeconds(0.63f);
        anim.SetTrigger("Respawn");
        speed = 18f;
        torch.intensity = 0f;

        Respawn();
        
        
        

    }


    private IEnumerator Intocable()
    {
        damageReceived = 0;
        isOnPlatform = true;
        yield return new WaitForSeconds(2f);
        damageReceived = 1;

    }

    private IEnumerator LightHitPos()
    {
        torch.transform.position = hitCheck.transform.position;
        anim.SetTrigger("Hit");
        yield return new WaitForSeconds(0.63f);
        torch.transform.position = torchInitPos;
    }

    private void Hit()
    {

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            //Lista de objetos en contacto -> en contacto con este círculo
            Collider2D[] objectsInContact = Physics2D.OverlapCircleAll(hitCheck.transform.position, hitCheckRadius);

            foreach (Collider2D col in objectsInContact) //Pasamos por todos los objetos en contacto
            {
                if (col.CompareTag("Enemy"))
                {
                    col.transform.GetComponent<Enemy>().DamageReceive(hitDamage);
                    AudioManager.Instance.PlaySFX(2);
                }
                else { AudioManager.Instance.PlaySFX(16); }
            }

            AudioManager.Instance.PlaySFX(0);
            
            StartCoroutine(LightHitPos());

        }
        
    }

    private void AirHit()
    {
        if (!isGrounded && !isOnPlatform && Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.LeftControl))
        {
            //anim.SetTrigger("Hit");
            playerRb.AddForce(-Vector3.up * (jumpForce*5), ForceMode2D.Impulse);
            Hit();
            //Debug.Log("Air Hit");
        }
    }

    private void Hurt()
    {
        anim.SetTrigger("Hurt");
        AudioManager.Instance.PlaySFX(8);

    }

    //Gestión del daño recibido
    private void OnCollisionEnter2D(Collision2D collision)
    {
       

        if (collision.gameObject.CompareTag("Pickup"))
        {
            GameManager.Instance.Pickup();
        }

        if (collision.gameObject.CompareTag("Trampa"))
        {

            Hurt();
            lifeCounter = lifeCounter - damageReceived;
            Debug.Log(lifeCounter);
            torch.intensity = lifeCounter - damageReceived;
            StartCoroutine(Intocable());

        }

       
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Muerte"))
        {
            AudioManager.Instance.PlaySFX(17);
            lifeCounter = 0;
        }

        if (collision.gameObject.CompareTag("EnemyHit"))
        {

            Hurt();
            lifeCounter = lifeCounter - damageReceived;
            Debug.Log(lifeCounter);
            torch.intensity = lifeCounter - damageReceived;
            StartCoroutine(Intocable());

        }

        if (collision.gameObject.CompareTag("Trampa"))
        {

            Hurt();
            lifeCounter = lifeCounter - damageReceived;
            Debug.Log(lifeCounter);
            torch.intensity = lifeCounter - damageReceived;
            StartCoroutine(Intocable());

        }

        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            lifeCounter = 3;
            anim.SetInteger("Life", 3);
            anim.SetTrigger("Checkpoint");
            respawnPos = collision.transform.position;
            torch.intensity = lifeCounter - 1;

            //anim.SetTrigger("Checkpoint");

        }

        if (collision.gameObject.CompareTag("Water") && !isOnPlatform)
        {
            AudioManager.Instance.PlaySFX(12);
            AudioManager.Instance.PlayMusic(3);
            anim.SetInteger("Life", 1);
            Hurt();
            isOnWater = true;
            lifeCounter = 1;
            playerRb.gravityScale = .2f;
            speed = 9;
            torch.intensity = lifeCounter - 1;
            //multiplicadorSalto = 0.05f;
            jumpForce = 10;
        }

       
            
    }   

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            AudioManager.Instance.PlaySFX(12);
            AudioManager.Instance.PauseMusic(3);
            isOnWater = false;
            playerRb.gravityScale = 2;
            speed = 18;
            //multiplicadorSalto = 0.1f;
            jumpForce = 12;
        }
    }


    void OnDrawGizmos()
        {
        Gizmos.DrawCube(groundCheck.transform.position, groundCheckSize);
        Gizmos.DrawCube(wallCheck.transform.position, wallCheckSize);
        Gizmos.DrawWireSphere(hitCheck.transform.position,hitCheckRadius);
        }
    }
