using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IceBlock : MonoBehaviour
{
    Rigidbody2D iceRb;
    [SerializeField] LayerMask endLayer;
    [SerializeField] GameObject iceCheck;
    [SerializeField] private float iceCheckRadius;
    [SerializeField]private Vector2 initialPos;
    [SerializeField]private bool isEnd;
    [SerializeField] GameObject colObject;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.position;
        iceCheck = GameObject.Find("IceCheck");
        iceRb = gameObject.GetComponent<Rigidbody2D>();
        iceCheckRadius = .7f;
    }

    // Update is called once per frame
    void Update()
    {
        isEnd = Physics2D.OverlapCircle(iceCheck.transform.position, iceCheckRadius, endLayer);
        Debug.Log(isEnd);

        if (isEnd)
        {
            Debug.Log(isEnd);
            StartCoroutine(Fall());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            transform.position = initialPos;
        }
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(2);
        iceRb.velocity = new Vector2(0,0); 
        transform.position = initialPos;
        Debug.Log(initialPos);
    }

    void OnDrawGizmos()
    {
        
        Gizmos.DrawWireSphere(iceCheck.transform.position, iceCheckRadius);
    }
}
