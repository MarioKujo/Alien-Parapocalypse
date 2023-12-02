using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Level : MonoBehaviour
{
    private SoundManager soundManager;//Controlador de sonidos
    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();//Busca el componente con el script en escena
    }
    private void Update()
    {
        soundManager.SeleccionAudio(4, 0.05f);//Pone la música
    }
}
