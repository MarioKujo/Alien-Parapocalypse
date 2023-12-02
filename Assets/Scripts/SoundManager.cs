using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] audios;//Array de clips de audio para elegir
    private AudioSource controlAudio;//Fuente de audio
    private void Awake()
    {
        controlAudio = GetComponent<AudioSource>();//Recibe el componente del objeto
    }
    public void SeleccionAudio (int indice, float volumen)//Función para seleccionar un audio con la posición del array y el volumen
    {
        controlAudio.PlayOneShot(audios[indice], volumen);//Reproduce un sonido con un clip (audios[indice]) y un volumen determinado
    }
    public float clipLength(int indice)
    {
        return audios[indice].length;//Devuelve la longitud del clip según su índice
    }
}
