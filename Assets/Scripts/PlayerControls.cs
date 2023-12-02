using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Objeto en la escena de controles (debajo de la tecla "O")
public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    GameObject fruta;//Recibe la fruta de controles
    [SerializeField]
    Canvas canvas;//Recibe el canvas en el que est�
    [HideInInspector]
    public bool frutaSpawneada = false;//revisa si hay una fruta spawneada
    private void FixedUpdate()
    {
        if (!frutaSpawneada)//Si no est� spawneada
        {
            GameObject clon = Instantiate(fruta, canvas.transform);//La instancia tomando el canvas como padre
            //Pone la fruta por detr�s del personaje en la jerarqu�a del canvas
            clon.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            clon.transform.position = transform.position;//La fruta se pone en la posici�n del jugador
            frutaSpawneada = true;//El booleano se vuelve verdadero
        }
    }
}
