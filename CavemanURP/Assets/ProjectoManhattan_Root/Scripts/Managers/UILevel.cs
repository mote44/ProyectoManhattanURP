using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UILevel : MonoBehaviour
{
    private int totalLifes;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject[] vidas;
    public float health;
    public float maxHealth;


    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 3; 
    }

    // Update is called once per frame
    void Update()
    {
        totalLifes = GameManager.Instance.superLifes;
        Debug.Log("Total Lifes " +  totalLifes);

       /*
        health = totalLifes / 3;

        if (health <= 0f) { health = 0f; }
        healthBar.fillAmount = health;
        Debug.Log("Health " + health);
       */

        if (totalLifes <= 0)
        {
            gameOverPanel.SetActive(true);
        }

        DesactivarVida();
        
    }

    public void DesactivarVida()
    {
        vidas[GameManager.Instance.superLifes].gameObject.SetActive(false);
    }

    

}
