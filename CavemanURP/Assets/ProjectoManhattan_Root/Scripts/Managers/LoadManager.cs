using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//Necesaria!!!!
public class LoadManager : MonoBehaviour
{
    [Header("Scene Management")]
    [SerializeField] int sceneToLoad; // Número para cargar la escena asociada al espacio en memoria del mismo número.
    //[SerializeField] int sceneRandomLoad; //Número resultante de una randomización entre la cantidad de escenas del juego
    //[SerializeField] int sceneQuantity; //Número para definir el maximo de escenas en el juego

    public void LoadScene()
    {
        //Sistema de carga de escena fia por una variable
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadSpecificScene(int sceneNumber)
    {
        //Carga una escena específica cambiando el valor numérico de referencia por cada llamada.
        SceneManager.LoadScene(sceneNumber); 
    }

    /*
    public void loadRandomScene()
    {
        //Sistema de carga de escenas random
        sceneRandomLoad = Random.Range(1,sceneQuantity);
        SceneManager.LoadScene(sceneRandomLoad);
    }
    */

    public void ExitGame()
    {
        //Cierra la aplicación al ser llamada como instrucción
        Application.Quit();
    }
    
   
}
