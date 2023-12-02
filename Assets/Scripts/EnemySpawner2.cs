using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemySpawner2 : MonoBehaviour
{
    [SerializeField]
    GameObject Enemigo;//recibe el prefab del enemigo
    [SerializeField]
    GameObject Inicio;//Primera coordenada x para empezar a generar enemigos
    [SerializeField]
    GameObject Final;//�ltima coordenada x para empezar a generar enemigos
    [SerializeField]
    float spawnTime; 
    float temporizador;//variable temporizador
    [SerializeField]
    int EnemigosTotales;//N�mero de enemigos que pueden spawnear al mismo tiempo en el nivel 1
    [HideInInspector]
    public int enemigosNivel;//N�mero de enemigos que pueden spawnear al mismo tiempo en el nivel actual
    public static int EnemigosActuales;//Contador de los enemigos actuales en escena
    private void Awake()
    {
        enemigosNivel = EnemigosTotales;//Se iguala el n�mero de enemigos totales del nivel 1 al contador de enemigos por nivel actual
        temporizador = spawnTime;//Se iguala el temporizador al tiempo de spawn introducido en el editor
    }
    private void FixedUpdate()
    {
        if (temporizador == 0)//si el temporizador es cero, crea el enemigo en un punto aleatorio del cielo
        {
            GameController.enemigosSpawneados++;//Aumenta el n�mero de enemigos spawneados en total
            //Crea un punto aleatorio entre el punto de inicio y el punto de final
            float a = Random.Range(Inicio.transform.position.x, Final.transform.position.x);//Genera un n�mero aleatorio entre las coordenadas x de inicio y fin
            float b = Random.Range(Inicio.transform.position.y, Final.transform.position.y);
            GameObject clon = Instantiate(Enemigo);//crea una instancia del enemigo
            clon.transform.position = new Vector3(a, b, 0);//pone el enemigo en la posici�n indicada
            EnemigosActuales++;//Aumenta el n�mero de enemigos spawneados en la escena actualmente
            temporizador = spawnTime;//Resetea el temporizador
        }
        //Si el temporizador es mayor de 0, el n�mero de enemigos actuales no es igual al n�mero de enemigos totales que se pueden spawnear y
        //el n�mero de enemigos spawneados este nivel no es igual al n�mero de enemigos totales por nivel, sigue el contador
        else if (temporizador > 0 && EnemigosActuales != enemigosNivel && GameController.enemigosSpawneados != GameController.enemigosNivel)
        {
            temporizador -= Time.fixedDeltaTime;
        }
        else if (temporizador < 0)//por si por alg�n motivo llega a ser menor de cero, vuelve a cero
        {
            temporizador = 0;
        }
    }
}