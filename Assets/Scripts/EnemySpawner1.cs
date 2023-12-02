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
    Transform RSpawner;//�ltima coordenada x para empezar a generar enemigos
    [SerializeField]
    float spawnTime;//Tiempo de spawneo de los enemigos
    float temporizador;//variable temporizador
    [SerializeField]
    int EnemigosTotales;//N�mero de enemigos que pueden spawnear al mismo tiempo en el nivel 1
    [HideInInspector]
    public int enemigosNivel;//N�mero de enemigos que pueden spawnear al mismo tiempo en el nivel actual
    public static int EnemigosActuales;//Enemigos que faltan por matar antes de pasar de nivel
    private void Awake()
    {
        enemigosNivel = EnemigosTotales;//Los enemigos del nivel 1 son iguales que los totales
        temporizador = spawnTime;//Se iguala el temporizador al tiempo de spawn para que no spawnee al empezar el nivel
    }
    private void FixedUpdate()
    {
        if (temporizador == 0)
            //Si el temporizador es 0 y el n�mero de enemigos spawneados hasta ahora no es igual que el n�mero total de enemigos
            //que tienen que spawnear por nivel, contin�a
        {
            GameController.enemigosSpawneados++;//Aumenta el n�mero de enemigos spawneados por nivel
            int a = Random.Range(0, 2);//Elige un n�mero aleatorio entre 0 y 1
            if (a == 0)//Si el n�mero es 0, elige el spawner izquierdo
            {
                GameObject clon = Instantiate(Enemigo);//crea una instancia del enemigo
                clon.transform.position = LSpawner.position;//pone el enemigo en la posici�n indicada
                EnemigosActuales++;//Aumenta el n�mero de enemigos actuales
                temporizador = spawnTime;//resetea el temporizador
            }
            else//Si no, elige el derecho
            {
                GameObject clon = Instantiate(Enemigo);//crea una instancia del enemigo
                clon.transform.position = RSpawner.position;//pone el enemigo en la posici�n indicada
                EnemigosActuales++;
                temporizador = spawnTime;//resetea el temporizador
            }
        }
        else if (temporizador > 0 && EnemigosActuales != enemigosNivel && GameController.enemigosSpawneados != GameController.enemigosNivel)//Si el temporizador est� activo y
                                                                       //el n�mero de enemigos en escena no es igual al n�mero de enemigos totales
        {
            temporizador -= Time.fixedDeltaTime;//Resta tiempo al temporizador hasta que llega a 0
        }
        else if (temporizador < 0)//por si por alg�n motivo llega a ser menor de cero, vuelve a cero
        {
            temporizador = 0;
        }
    }
}