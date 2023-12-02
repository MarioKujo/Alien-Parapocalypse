using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject canvasSeguro;//Recibe el canvas que pregunta "Estás seguro?"
    [SerializeField]
    Text record;//Recibe el texto del récord actual
    private void Awake()
    {
        record.text = "Current record: " + HUDManager.Record;//Cambia el texto del récord actual
        canvasSeguro.SetActive(false);//Desactiva el canvas
    }
    public void CargarEscena(string SceneName)
    {
        SceneManager.LoadScene(SceneName);//Carga la escena que se introduzca desde el editor
    }
    public void Seguro()//Usado en el botón Quit del menú principal
    {
        canvasSeguro.SetActive(true);//Se activa el canvas que revisa si estás seguro de tu selección
    }
    public void Return()//Usado en "no"
    {
        canvasSeguro.SetActive(false);//Desactiva el canvas
    }
    public void Salir()//Usado en "Yes"
    {
        Application.Quit();//Cierra la aplicación
    }
}
