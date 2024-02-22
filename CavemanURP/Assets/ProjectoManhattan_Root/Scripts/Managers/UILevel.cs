using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UILevel : MonoBehaviour
{
    private int totalLifes;
    [SerializeField] TMP_Text pointsText;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject[] vidas;
    public float health;
    public float maxHealth;
    public bool winner;


    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 3;
        winner = false;
    }

    // Update is called once per frame
    void Update()
    {
        totalLifes = GameManager.Instance.superLifes;
        winner = GameManager.Instance.gameCompleted;
        Debug.Log("Total Lifes " +  winner);

       /*
        health = totalLifes / 3;

        if (health <= 0f) { health = 0f; }
        healthBar.fillAmount = health;
        Debug.Log("Health " + health);
       */

        if (totalLifes <= 0)
        {
            
            StartCoroutine(BackToMenu());
        }

       

        if(winner==true)
        {
            winPanel.SetActive(true);
            Debug.Log("Total Lifes " + winner);
        }

        pointsText.text = "Congratulations,you've found " + GameManager.Instance.pickupPoints.ToString() + " Venus figures";

        DesactivarVida();

    }

    public void DesactivarVida()
    {
        vidas[GameManager.Instance.superLifes].gameObject.SetActive(false);
    }


    private IEnumerator BackToMenu()
    {
        gameOverPanel.SetActive(true);
        AudioManager.Instance.PlaySFX(13);
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(0);
    }
    public void GameCompleted()
    {
        winPanel.SetActive(true);
        
    }
}
