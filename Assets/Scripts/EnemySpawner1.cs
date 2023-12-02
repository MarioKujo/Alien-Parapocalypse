using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemySpawner1 : MonoBehaviour
{
    [SerializeField]
    GameObject Enemigo;//recibe el prefab del enemigo
    [SerializeField]
    Transform LSpawner;//Primera coordenada x para empezar a generar enemigos
    [SerializeField]
    Transform RSpawner;//Última coordenada x para empezar a generar enemigos
    [SerializeField]
    float spawnTime;//Tiempo de spawneo de los enemigos
    float temporizador;//variable temporizador
    [SerializeField]
    int EnemigosTotales;//Número de enemigos que pueden spawnear al mismo tiempo en el nivel 1
    [HideInInspector]
    public int enemigosNivel;//Número de enemigos que pueden spawnear al mismo tiempo en el nivel actual
    public static int EnemigosActuales;//Enemigos que faltan por matar antes de pasar de nivel
    private void Awake()
    {
        enemigosNivel = EnemigosTotales;//Los enemigos del nivel 1 son iguales que los totales
        temporizador = spawnTime;//Se iguala el temporizador al tiempo de spawn para que no spawnee al empezar el nivel
    }
    private void FixedUpdate()
    {
        if (temporizador == 0)
            //Si el temporizador es 0 y el número de enemigos spawneados hasta ahora no es igual que el número total de enemigos
            //que tienen que spawnear por nivel, continúa
        {
            GameController.enemigosSpawneados++;//Aumenta el número de enemigos spawneados por nivel
            int a = Random.Range(0, 2);//Elige un número aleatorio entre 0 y 1
            if (a == 0)//Si el número es 0, elige el spawner izquierdo
            {
                GameObject clon = Instantiate(Enemigo);//crea una instancia del enemigo
                clon.transform.position = LSpawner.position;//pone el enemigo en la posición indicada
                EnemigosActuales++;//Aumenta el número de enemigos actuales
                temporizador = spawnTime;//resetea el temporizador
            }
            else//Si no, elige el derecho
            {
                GameObject clon = Instantiate(Enemigo);//crea una instancia del enemigo
                clon.transform.position = RSpawner.position;//pone el enemigo en la posición indicada
                EnemigosActuales++;
                temporizador = spawnTime;//resetea el temporizador
            }
        }
        else if (temporizador > 0 && EnemigosActuales != enemigosNivel && GameController.enemigosSpawneados != GameController.enemigosNivel)//Si el temporizador está activo y
                                                                       //el número de enemigos en escena no es igual al número de enemigos totales
        {
            temporizador -= Time.fixedDeltaTime;//Resta tiempo al temporizador hasta que llega a 0
        }
        else if (temporizador < 0)//por si por algún motivo llega a ser menor de cero, vuelve a cero
        {
            temporizador = 0;
        }
    }
}