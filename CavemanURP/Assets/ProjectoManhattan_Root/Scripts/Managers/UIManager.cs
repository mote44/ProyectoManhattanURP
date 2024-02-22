using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{

    [SerializeField] GameObject optPanel;
    [SerializeField] GameObject playBtn;
    [SerializeField] GameObject exitBtn;
    [SerializeField] GameObject optBtn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowOptPanel()
    {
        optPanel.SetActive(true);
        
        playBtn.GetComponent<Button>().enabled = false;
        exitBtn.GetComponent<Button>().enabled = false;
        optBtn.GetComponent<Button>().enabled = false;
    }

    public void HideOptPanel()
    {
        optPanel.SetActive(false);
        playBtn.GetComponent<Button>().enabled = true;
        exitBtn.GetComponent<Button>().enabled = true;
        optBtn.GetComponent<Button>().enabled = true;
    }
}
