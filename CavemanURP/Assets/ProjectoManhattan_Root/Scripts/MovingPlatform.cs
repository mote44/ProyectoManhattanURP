using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private int i; //�ndice del array de puntos para que la plataforma detecte un punto a seguir.
    [Header("Point & Movement Configuration")]
    [SerializeField] Transform[] points; //Array de puntos de posici�n hacia los que la plataforma se mover�.
    [SerializeField] int startingPoint; //N�mero para determinar el �ndice del punto de inicio de la plataforma.
    [SerializeField] float speed; //Velocidad de la plataforma.

    // Start is called before the first frame update
    void Start()
    {
        //Setear la posici�n inicial de la plataforma a uno de los puntos, asignando a startingPoint un valor num�rico.
        transform.position = points[startingPoint].position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++; //Aumenta el �ndice, cambia de objetivo hacia el que moverse.
            if (i == points.Length) //Chequea si la plataforma ha llegado al �ltimo punto del array.
            {
                i = 0; //Resetea el �ndice para volver a empezar, la plataforma va hacia el punto 0.
            }
        }


        //Mueve la plataforma a la posici�n del punto guardado en el array...
        //... que corresponda al espacio del array con valor igual a la variable "i"
        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (transform.position.y < collision.transform.position.y) 
            {
                //El transform del objeto que ha colisionado (player) se hace hijo del transform de la plataforma.
                collision.transform.SetParent(transform);
            }
            

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            //El transform del objeto que sale de la colisi�n (player) pierde su condici�n de hijo del transform de la plataforma.
            collision.transform.SetParent(null);
        }
    }
}
