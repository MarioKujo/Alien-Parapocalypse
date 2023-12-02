using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUDManager : MonoBehaviour
{
    [SerializeField]
    Text vidaTexto;//Recibe el texto que dice la vida que le queda al jugador
    [SerializeField]
    Text puntuacionTexto;//Puntuación total
    [SerializeField]
    Text nivelActual;//Nivel actual
    [SerializeField]
    Text enemigosRestantes;//Número de enemigos restantes antes de subir de nivel
    public static int Record;//Record actual
    public static int puntuacionActual;//Puntuación actual
    public static bool newRecord = false;//Si es nuevo record o no
    private void FixedUpdate()
    {
        puntuacionTexto.text = "Score: " + puntuacionActual;//Cambia el texto de puntuación
        vidaTexto.text = "Life: " + MovimientoJugador.vidaActual;//Cambia el texto de vida
        //Cambia el texto de los enemigos restantes antes de cambiar de nivel
        enemigosRestantes.text = "Enemies remaining: " + GameController.enemigosActuales;
        nivelActual.text = "Level: " + GameController.nivelActual;//Cambia el texto del nivel actual
    }
}
