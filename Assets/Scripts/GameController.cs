using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    GameObject enemigo1;//Recibe el enemigo1
    [SerializeField]
    GameObject enemigo2;//Recibe el enemigo2
    [SerializeField]
    GameObject enemigo3;//Recibe el enemigo3
    [SerializeField]
    GameObject gameOver;//Recibe el canvas de fin de partida
    [SerializeField]
    GameObject canvasPausa;//Menú de pausa
    [SerializeField]
    GameObject canvasSeguro;//Menú que te pregunta "Seguro? Sí/No"
    [SerializeField]
    GameObject HUD;//Recibe el HUD
    [SerializeField]
    Text actualScore;//Recibe la puntuación actual
    [SerializeField]
    Text recordText;//Recibe el récord actual
    [SerializeField]
    GameObject newRecord;//Recibe el texto "Nuevo récord!"
    string NombreEscena = "";//Para introducir el nombre de la escena
    MovimientoJugador mj1;//Jugador 1
    [SerializeField]
    int enemigosTotales;//Enemigos totales del primer nivel
    #region public static
    public static int enemigosNivel;//Enemigos totales del nivel actual
    public static int enemigosActuales;//Enemigos actuales que faltan por matar
    public static int nivelActual = 1;//Nivel actual
    public static bool cambiaNivel;//Booleano que revisa si se cambia de nivel
    public static int enemigosSpawneados;//enemigos spawneados en total
    #endregion
    [HideInInspector]
    public bool isGameOver = false;//Revisa si se activa la pantalla de game over
    bool messageSent = false;
    void Start()
    {
        newRecord.SetActive(false);//Desactiva el texto de nuevo récord
        mj1 = FindObjectOfType<MovimientoJugador>();//Busca el jugador 1 en la escena
        enemigosNivel = enemigosTotales;//Los enemigos totales del nivel actual se igualan al número total de los enemigos del nivel 1
        enemigosActuales = enemigosNivel;//Los enemigos actuales se actualizan para que sea igual que el número de enemigos en el nivel actual
        canvasPausa.SetActive(false);//inicializa el canvas para que sea invisible
        canvasSeguro.SetActive(false);//inicializa el canvas para que sea invisible
        gameOver.SetActive(false);//Desactiva el canvas de fin de partida
    }
    void FixedUpdate()
    {
        if (enemigosSpawneados == enemigosNivel && !messageSent)
        {
            Debug.Log("enemigosSpawneados == enemigosNivel");
            messageSent = true;
        }
        if (enemigosActuales == 0)//Si los enemigos actuales son 0
        {
            SiguienteNivel();//Se cambia el nivel
        }
        //Si se pulsa la tecla escape y todavía no es fin de partida, se activa el menú de pausa y se para todo el juego
        if (Input.GetKey(KeyCode.Escape) && !isGameOver)
        {
            Pausa();//Función para pausar
        }
    }
    public void Pausa()
    {
        if (!isGameOver)
        {
            canvasSeguro.SetActive(false);//Desactiva el seguro (por si acaso le da a No y vuelve al anterior)
            canvasPausa.SetActive(true);//Vuelve el de pausa activo
        }
        else
        {
            canvasSeguro.SetActive(false);//Desactiva el seguro
            canvasPausa.SetActive(false);//Desactiva el de pausa
            gameOver.SetActive(true);//Activa el de final de partida
        }
        Time.timeScale = 0;//Se detiene el tiempo
    }
    public void Reanudar()//Si se activa esta función, el menú de pausa desaparece y vuelve a correr el tiempo
    {
        canvasPausa.SetActive(false);//Se desactiva el canvas de pausa
        Time.timeScale = 1;//Y se reanuda el tiempo
    }
    public void Seguro(string Nombre)//Pregunta si estás seguro
    {
        NombreEscena = Nombre;//Introduce el nombre de la escena (si pulsas el botón "Menú principal" es "MainMenu"
                              //y si le das a "Salir" cierra el programa)
        canvasSeguro.SetActive(true);//Activa el canvas "Seguro?"
        canvasPausa.SetActive(false);//Desactiva el canvas pausa
        gameOver.SetActive(false);//Desactiva el de final de partida
    }
    /*Los he tenido que hacer separados porque el "Seguro" se lo aplico a los botones "Menú principal" y "Salir", y el CargarEscena a los botones
     "Sí" y "No"*/
    public void CargarEscena()
    {
        if (NombreEscena == "MainMenu")//Si es al menú principal
        {
            Time.timeScale = 1;//Se reanuda el tiempo
            EnemySpawner1.EnemigosActuales = 0;//Se reinicia el número de enemigos spawneados a 0
            EnemySpawner2.EnemigosActuales = 0;//Se reinicia el número de enemigos spawneados a 0
            EnemySpawner3.EnemigosActuales = 0;//Se reinicia el número de enemigos spawneados a 0
            MovimientoEnemigo1.inicializado = false;//Se vuelve falso el bool de inicializado
            MovimientoEnemigo2.inicializado = false;//Se vuelve falso el bool de inicializado
            MovimientoEnemigo3.inicializado = false;//Se vuelve falso el bool de inicializado
            nivelActual = 1;//El nivel actual se reincia a 1
            HUDManager.puntuacionActual = 0;//La punutación actual se reinicia a 0
            HUDManager.newRecord = false;//El nuevo récord se reinicia a falso
            enemigosSpawneados = 0;//El número de enemigos spawneados en el nivel en total se vuelve 0
            MovimientoJugador.vidaActual = mj1.vida;//La vida actual se reinicia a la vida por defecto
            SceneManager.LoadScene("Title");//Si recibe "MainMenu", vuelve al título
        }
        if (NombreEscena == "Quit")
        {
            Cerrar();//Si recibe "Quit", se sale
        }
    }
    public void Cerrar()
    {
        Application.Quit();//Opción para salir del juego
    }
    public void Reiniciar()
    {
        messageSent = false;
        enemigosSpawneados = 0;
        Time.timeScale = 1;//Se reinicia el tiempo
        EnemySpawner1.EnemigosActuales = 0;//Se reinicia el número de enemigos spawneados a 0
        EnemySpawner2.EnemigosActuales = 0;//Se reinicia el número de enemigos spawneados a 0
        EnemySpawner3.EnemigosActuales = 0;//Se reinicia el número de enemigos spawneados a 0
        MovimientoEnemigo1.inicializado = false;//Se vuelve falso el bool de inicializado
        MovimientoEnemigo2.inicializado = false;//Se vuelve falso el bool de inicializado
        MovimientoEnemigo3.inicializado = false;//Se vuelve falso el bool de inicializado
        nivelActual = 1;//El nivel actual se reincia a 1
        HUDManager.puntuacionActual = 0;//La punutación actual se reinicia a 0
        HUDManager.newRecord = false;//No hay nuevo récord
        MovimientoJugador.vidaActual = mj1.vida;//La vida actual se reinicia a la vida por defecto
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GameOver()
    {
        HUD.SetActive(false);//Desactiva el HUD
        actualScore.text = "Score: " + HUDManager.puntuacionActual;//Cambia el texto de la puntuación actual
        recordText.text = "Record: " + HUDManager.Record;//Cambia el texto de la puntuación actual
        if (HUDManager.newRecord)
        {
            newRecord.SetActive(true);//Si es nuevo récord, se activa el texto
        }
        Time.timeScale = 0;//El tiempo se pausa
        isGameOver = true;//El booleano se vuelve verdadero
        canvasSeguro.SetActive(false);//Se desactiva el canvas "Seguro?"
        canvasPausa.SetActive(false);//Se desactiva el canvas pausa
        gameOver.SetActive(true);//Se activa el canvas de final de partida
    }
    #region Cambiar nivel
    void SiguienteNivel()
    {
        messageSent = false;
        enemigosSpawneados = 0;
        cambiaNivel = true;//Activa el booleano
        enemigosNivel += 5;//Aumenta en 5 el número de enemigos totales
        enemigosActuales = enemigosNivel;//Se vuelve a igualar el número de enemigos actuales a los que hay en el nivel
        nivelActual++;//Aumenta el nivel actual
        CambiarEnemigo1();
        CambiarEnemySpawner1();
        CambiarEnemigo2();
        CambiarEnemySpawner2();
        CambiarEnemigo3();
        CambiarEnemySpawner3();
        cambiaNivel = false;//Se resetea a falso
    }
    void CambiarEnemigo1()
    {
        MovimientoEnemigo1.retardoNivel -= 0.01f;//Se disminuye el tiempo que tarda en girarse
        MovimientoEnemigo1.velocidadNivel += 0.1f;//Aumenta la velocidad
    }
    void CambiarEnemySpawner1()
    {
        EnemySpawner1 es1 = FindObjectOfType<EnemySpawner1>();//Busca en escena el objeto con este script
        es1.enemigosNivel += 1;//Aumenta en 1 el número de enemigos que pueden spawnear
    }
    void CambiarEnemigo2()
    {
        MovimientoEnemigo2.velocidadNivel += 0.01f;//Se aumenta la velocidad a la que se mueve el enemigo 2 cada nivel
    }
    void CambiarEnemySpawner2()
    {
        EnemySpawner2 es2 = FindObjectOfType<EnemySpawner2>();//Busca en escena el objeto con este script
        es2.enemigosNivel += 1;//Aumenta en 1 el número de enemigos que pueden spawnear
    }
    void CambiarEnemigo3()
    {
        if (cambiaNivel)
        {
            MovimientoEnemigo3.levelSpeed += 0.01f;//Se aumenta la velocidad de movimiento de los enemigos
            MovimientoEnemigo3.velocidadBalaNivel += 0.01f;//Se aumenta la velocidad a la que viaja la bala
        }
    }
    void CambiarEnemySpawner3()
    {
        EnemySpawner3 es3 = FindObjectOfType<EnemySpawner3>();//Busca en escena el objeto con este script
        es3.enemigosNivel += 1;//Aumenta en 1 el número de enemigos que pueden spawnear
    }
    #endregion
}