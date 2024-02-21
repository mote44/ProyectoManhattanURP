using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIManager : MonoBehaviour
{

    [SerializeField] GameObject optPanel;
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
    }

    public void HideOptPanel()
    {
        optPanel.SetActive(false);
    }
}
