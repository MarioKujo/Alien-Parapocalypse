using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MovimientoEnemigo3 : MonoBehaviour
{
    [SerializeField]
    GameObject attackPosition;//Posición de ataque
    [SerializeField]
    GameObject puntuacion;//El texto que spawnea
    [SerializeField]
    float moveSpeed;//Velocidad de movimiento
    public static float levelSpeed;//Velocidad de movimiento que aumenta por cada nivel
    [SerializeField]
    GameObject bala;//Prefab de la bala
    float temporizador;//Temporizador actual
    [SerializeField]
    float velocidadBala;//Velocidad de la bala
    public static float velocidadBalaNivel;//Velocidad de la bala que aumenta cada nivel
    EnemySpawner3 es3;//Para saber adonde ir
    Animator mechanim;//Animador
    MovimientoJugador mj1;
    Rigidbody2D rb2D;
    bool isDying;
    public static bool inicializado;//Revisa si está inicializado por primera vez para igualar todo al nivel 1
    private void Awake()
    {
        if (!inicializado)//Si no está inicializado
        {
            levelSpeed = moveSpeed;//La velocidad de movimiento que varía se iguala al valor del primer nivel
            velocidadBalaNivel = velocidadBala;//La velocidad de la bala que varía se iguala al valor del primer nivel
            inicializado = true;
        }
        temporizador = Random.Range(0.5f, 7.9f);//Empieza un temporizador aleatorio para spawnear una bala entre medio segundo y 8
        rb2D = GetComponent<Rigidbody2D>();//Recibe el rigidbody
        mj1 = FindObjectOfType<MovimientoJugador>();//Busca al jugador en la escena
        mechanim = GetComponent<Animator>();//Recibe el componente de dentro del objeto
        es3 = FindObjectOfType<EnemySpawner3>();//Encuentra el objeto con el scriptç
        attackPosition.SetActive(false);//Desactiva el ataque
    }
    private void FixedUpdate()
    {
        if (!isDying)//Si no se está muriendo
        {
            if (mj1.transform.position.x > transform.position.x)//Si el jugador está a la izquierda
            {
                transform.eulerAngles = new Vector3(0, 180, 0);//Se gira
            }
            else//Si no
            {
                transform.eulerAngles = Vector3.zero;//No se gira
            }
            if (temporizador > 0)//Si el temporizador de spawn de la bala es mayor que 0
            {
                temporizador -= Time.fixedDeltaTime;//El temporizador sigue contando
            }
            if (temporizador <= 0)//Si se acaba el temporizador
            {
                GameObject clon = Instantiate(bala);//Instancia una bala
                clon.transform.position = transform.position;//En la posición del enemigo
                clon.GetComponent<Rigidbody2D>().velocity = Vector2.down * velocidadBalaNivel;//Y hacia abajo
                temporizador = Random.Range(0.5f, 7.9f);//Y vuelve a empezar el temporizador aleatorio
            }
            transform.position = Vector2.MoveTowards(transform.position, es3.EndPoint.position, levelSpeed);//Se mueve desde el inicio hasta el final (ambos indicados en el editor)
            if (es3.EndPoint.position.x <= transform.position.x)//Si llega al final o se pasa del final
            {
                //Los enemigos spawneados en total se reducen en 1 porque eso significa que el jugador no lo ha matado
                GameController.enemigosSpawneados--;
                EnemySpawner3.EnemigosActuales--;//Los enemigos actuales máximos del spawner 2 se reducen en 1
                Destroy(gameObject);//Simplemente se destruye, sin animación ni nada
            }
        }
    }
    public void Destruirse()//Se destruye con animación
    {
        gameObject.layer = 2;
        isDying = true;//Se vuelve verdadero para que no se mueva ni genere balas
        transform.position = transform.position;//Se iguala su posición para que deje de moverse
        mechanim.SetTrigger("Destroy");//Se cambia el trigger de la animación
        AnimatorStateInfo stateInfo = mechanim.GetCurrentAnimatorStateInfo(0);//Se recibe la animación actual
        float duration = stateInfo.length;//Recibe la longitud de la animación actual
        StartCoroutine(DestruirDespuesDe(duration, rb2D));//Empieza la corutina de destruir
    }
    //Este recibe un rigidbody para que no haya confusión entre varios enemigos del mismo tipo
    IEnumerator DestruirDespuesDe(float tiempo, Rigidbody2D rb2D)
    {
        rb2D.gravityScale = 1;//Activa la gravedad
        rb2D.velocity = Vector2.zero;//Resetea la velocidad (para que no suba después de darle con la bala)
        rb2D.AddForce(Vector2.down * 15, ForceMode2D.Impulse);//Es arrastrado al suelo
        yield return new WaitForSeconds(tiempo);//Espera un segundo
        rb2D.velocity = Vector2.zero;//Devuelve la velocidad aplicada a 0
        rb2D.gravityScale = 0;//Le quita la gravedad
        mechanim.SetTrigger("Explode");//Cambia el trigger de animación
        attackPosition.SetActive(true);//Se activa el trigger de ataque
        AnimatorStateInfo stateinfo = mechanim.GetCurrentAnimatorStateInfo(0);//Recibe la información de la animación actual
        yield return new WaitForSeconds(stateinfo.length);//Espera los segundos que tarde en ejecutarse la animación
        EnemySpawner3.EnemigosActuales--;//Los enemigos totales bajan en 1
        HUDManager.puntuacionActual += 1000;//La puntuación aumenta en 1000
        puntuacion.GetComponent<TextMesh>().text = 1000.ToString();//El texto cambia a 1000
        GameObject clon = Instantiate(puntuacion);//Se instancia el texto de puntuación
        clon.transform.position = transform.position;//Se pone en la posición indicada
        GameController.enemigosActuales--;//Los enemigos actuales antes de pasar de nivel bajan en 1
        Destroy(gameObject);//Se destruye el objeto
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BalaJugador") && mechanim != null)
        {
            Destruirse();//Si el enemigo colisiona contra la bala del jugador, se destruye
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            mj1.RestarVida();//Si el jugador entra en colisión con el trigger del enemigo (ataque), le resta vida
        }
    }
}