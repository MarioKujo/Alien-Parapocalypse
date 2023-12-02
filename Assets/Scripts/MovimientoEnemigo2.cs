using System.Collections;
using UnityEngine;
public class MovimientoEnemigo2 : MonoBehaviour
{
    [SerializeField]
    Transform Attack;
    [SerializeField]
    GameObject puntuacion;//Recibe el texto de puntuación que va a salir
    [SerializeField]
    float velocidadEnemigo;//Introduce la velocidad a la que se mueve el enemigo
    public static float velocidadNivel;//velocidad de movimiento por nivel
    [SerializeField]
    float distanciaMaximaX;//El valor máximo de X al que se puede mover
    [SerializeField]
    float distanciaMaximaY;//El valor máximo de Y al que se puede mover
    [SerializeField]
    float distanciaMinimaY;//El valor mínimo de Y al que se puede mover
    [SerializeField]
    GameObject prefabBala;//El prefab de la bala que va a usar
    [SerializeField]
    float tiempoEntreDisparos;//Tiempo entre un disparo y otro en el nivel 1
    public static float tiempoNivel;//Tiempo entre un disparo y otro en el nivel actual
    float temporizador;//Temporizador actual
    Vector2 direccion;//Punto aleatorio al que tiene que moverse
    Animator mechanim;//Animador
    GameObject jugador;//Recibe una instancia del jugador
    Rigidbody2D rb2D;//Rigidbody para la muerte
    bool isDying;//Booleano para revisar si se está muriendo o no
    public static bool inicializado;//Booleano que revisa si ha sido inicializado por primera vez
    MovimientoJugador mj1;//Recibe el movimiento del jugador
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();//Recibe el rigidbody del enemigo
        if (!inicializado)//Si el enemigo no está inicializado
        {
            velocidadNivel = velocidadEnemigo;//Iguala la velocidad de movimiento del nivel actual a la del nivel 1
            tiempoNivel = tiempoEntreDisparos;//Iguala el tiempo entre disparos del nivel actual al del nivel 1
            inicializado = true;//cambia el booleano a verdadero
        }
        mj1 = FindObjectOfType<MovimientoJugador>();//Busca el objeto en la escena con el script
    }
    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player");//Recibe el gameobject del objeto con la etiqueta Player
        mechanim = GetComponent<Animator>();//Recibe el componente animador del propio enemigo
        temporizador = tiempoNivel;//Empieza el temporizador
    }
    private void FixedUpdate()
    {
        if (!isDying)//Si no se está muriendo
        {
            if (jugador.transform.position.x > transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);//Si el jugador está a la derecha, se gira
            }
            else
            {
                transform.eulerAngles = Vector3.zero;//Si el jugador está a la izquierda, se gira
            }
            if (Vector2.Distance(direccion, new Vector2(transform.position.x, transform.position.y)) < 0.1f)//Revisa si ha llegado a su destino
            {
                direccion = new Vector2(Random.Range(-distanciaMaximaX, distanciaMaximaX), Random.Range(distanciaMinimaY, distanciaMaximaY));//Crea un nuevo destino al que desplazarse
            }
            transform.position = Vector2.MoveTowards(transform.position, direccion, velocidadNivel * Time.fixedDeltaTime);//Se mueve con una velocidad fija cada segundo hacia el punto creado
            if (temporizador > 0)
            {
                temporizador -= Time.fixedDeltaTime;//Si el temporizador sigue siendo mayor de 0, continúa
            }
            if (temporizador <= 0)//Si llega a 0
            {
                mechanim.SetTrigger("Attack");//Cambia el trigger del animador
                temporizador = tiempoNivel;//Se reinicia el temporizador
            }   
        }
    }
    void Instanciar()//Usado como evento en el animator de Unity, Enemy2_Attack
    {
        GameObject clon = Instantiate(prefabBala);//Instancia una bala
        clon.transform.position = transform.position;//en la posición del enemigo
        //Crea un vector para medir la distancia entre el enemigo y el jugador
        Vector3 direccionDisparo = jugador.transform.position - transform.position;
        clon.GetComponent<EnemyBullet2>().Inicializar(direccionDisparo);//Recibe el componente de otro script para usar una función ahí dentro
    }
    public void Destruirse()//Se destruye
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
        rb2D.AddForce(Vector2.down * 10, ForceMode2D.Impulse);//Es arrastrado al suelo
        yield return new WaitForSeconds(tiempo);//Espera un segundo
        EnemySpawner2.EnemigosActuales--;//Los enemigos totales bajan en 1
        HUDManager.puntuacionActual += 500;//Aumenta la puntuación global
        puntuacion.GetComponent<TextMesh>().text = 500.ToString();//Cambia el texto a instanciar
        GameObject clon = Instantiate(puntuacion);//Instancia el texto
        clon.transform.position = transform.position;//Lo pone en su posición
        GameController.enemigosActuales--;//Disminuye el número de enemigos de tipo 2 en escena al mismo tiempo
        Destroy(gameObject);//Se destruye el objeto
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BalaJugador") && mechanim != null)
        {
            Destruirse();//Si colisionan contra la bala del jugador, se destruyen
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            mj1.RestarVida();//Si el trigger de ataque entra en contacto con el jugador, este recibe daño
        }
    }
}