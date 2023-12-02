using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Objeto en la escena de controles (debajo de la tecla "O")
public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    GameObject fruta;//Recibe la fruta de controles
    [SerializeField]
    Canvas canvas;//Recibe el canvas en el que está
    [HideInInspector]
    public bool frutaSpawneada = false;//revisa si hay una fruta spawneada
    private void FixedUpdate()
    {
        if (!frutaSpawneada)//Si no está spawneada
        {
            GameObject clon = Instantiate(fruta, canvas.transform);//La instancia tomando el canvas como padre
            //Pone la fruta por detrás del personaje en la jerarquía del canvas
            clon.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            clon.transform.position = transform.position;//La fruta se pone en la posición del jugador
            frutaSpawneada = true;//El booleano se vuelve verdadero
        }
    }
}
