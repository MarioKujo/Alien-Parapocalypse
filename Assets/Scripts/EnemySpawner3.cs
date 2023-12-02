using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemySpawner3 : MonoBehaviour
{
    [SerializeField]
    GameObject Enemigo;//recibe el prefab del enemigo
    [SerializeField]
    Transform StartPoint;//Punto de inicio
    public Transform EndPoint;//Punto de final
    [SerializeField]
    float spawnTime;//Tiempo de spawneo
    float temporizador;//variable temporizador
    [SerializeField]
    int EnemigosTotales;//Número total de enemigos que pueden spawnear al mismo tiempo en el nivel 1
    [HideInInspector]
    public int enemigosNivel;//número total de enemigos que pueden spawnear al mismo tiempo en el nivel actual
    public static int EnemigosActuales;//Número total de enemigos en escena actualmente
    private void Awake()
    {
        temporizador = 0;//SpawnNivel hace la misma función que enemigosNivel, está ahí para disminuir el tiempo entre spawns de enemigos
        enemigosNivel = EnemigosTotales;//Otra referencia al número total de enemigos que puede spawnear el spawner 3, sólo que se va a cambiar a la que aumente el nivel
    }
    private void FixedUpdate()
    {
        if (temporizador == 0)//si el temporizador es cero, crea el enemigo en un punto aleatorio del cielo
        {
            GameController.enemigosSpawneados++;//Aumenta el número de enemigos spawneados
            GameObject clon = Instantiate(Enemigo);//crea una instancia del enemigo
            clon.transform.position = new Vector3(StartPoint.position.x, transform.position.y, 0);//pone el enemigo en la posición indicada
            EnemigosActuales++;//Aumenta en uno el número de enemigos presentes en la escena
            temporizador = spawnTime;//Reinicia el temporizador
        }
        //Si el temporizador es mayor de 0, el número actual de enemigos en escena no es igual al número máximo o si el número
        //de enemigos spawneados en este nivel no es igual al número de enemigos totales que pueden spawnear en este nivel, el contador sigue
        else if (temporizador > 0 && EnemigosActuales != enemigosNivel && GameController.enemigosSpawneados != GameController.enemigosNivel)
        {
            temporizador -= Time.fixedDeltaTime;
        }
        else if (temporizador < 0)//por si por algún motivo llega a ser menor de cero, vuelve a cero
        {
            temporizador = 0;
        }
    }
}