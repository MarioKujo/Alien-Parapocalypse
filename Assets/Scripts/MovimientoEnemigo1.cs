using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MovimientoEnemigo1 : MonoBehaviour
{
    [SerializeField]
    GameObject puntuacion;//Texto que instancia después de morir
    [SerializeField]
    float velocidadEnemigo;//Velocidad del nivel 1
    public static float velocidadNivel;//Velocidad que aumenta cada nivel
    [SerializeField]
    LayerMask Ground;//Máscara de capa para revisar si está en el suelo
    [SerializeField]
    float retardoGiro;//Tiempo que tarda en girarse para moverse hacia el jugador en el nivel 1
    public static float retardoNivel;//Tiempo que tarda en girarse para moverse hacia el jugador en el nivel actual
    MovimientoJugador mj1;//Crea instancia movimientojugador para recibir sus coordenadas
    Rigidbody2D rb2D;//Rigidbody para moverse
    float timer;//Temporizador
    bool isRight;//Booleano que revisa si el jugador está a la derecha
    bool wasRight;//Booleano que revisa si el jugador estaba mirando hacia la derecha antes del cambio o no
    bool isDying;//Booleano que revisa si se está llevando a cabo la animación de muerte
    Animator mechanim;//Recibe el animador
    public static bool inicializado;//Booleano que revisa si se ha inicializado por primera vez
    private void Awake()
    {
        if (!inicializado)//Si se inicializa por primera vez
        {
            velocidadNivel = velocidadEnemigo;//se iguala la velocidad de movimiento del nivel actual a la del nivel 1
            retardoNivel = retardoGiro;//Se iguala el tiempo de retardo de giro del nivel actual al del nivel 1
            inicializado = true;//El booleano se vuelve verdadero
        }
        timer = 0;//El temporizador de giro se vuelve 0 nada más spawnea
        mechanim = GetComponent<Animator>();//Recibe el animador
        mj1 = FindObjectOfType<MovimientoJugador>();//mj1 busca el objeto del tipo movimientojugador
        rb2D = GetComponent<Rigidbody2D>();//Recibe las funciones de rigidbody2D
        isRight = mj1.transform.position.x > transform.position.x;//Recibe si el jugador está a la derecha
        wasRight = isRight;//Revisa si estaba a la derecha antes del giro o no
        if (isRight)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);//Si está a la derecha al principio, cae mirando hacia la derecha
        }
    }
    private void FixedUpdate()
    {
        if (!isDying)//Si no se está muriendo
        {
            Move();//Si está en el suelo, se mueve
        }
    }
    void Move()
    {
        isRight = mj1.transform.position.x > transform.position.x;//Booleano que revisa si el personaje está a la derecha
        if (isRight != wasRight)//Si cambia en algún momento
        {
            timer -= Time.fixedDeltaTime;//Empieza el temporizador
            if (timer <= 0)//Si el temporizador es menor o igual que 0
            {
                wasRight = isRight;//Se cambia el booleano
                timer = retardoNivel;//Se resetea el temporizador
                transform.eulerAngles = new Vector3(0, isRight ? 180 : 0, 0);//Dependiendo de si está a la derecha o no, se gira
            }
        }
        rb2D.velocity = new Vector2((wasRight ? 1 : -1) * velocidadNivel, rb2D.velocity.y);//Aplica una velocidad dependiendo de si está hacia la derecha o hacia la izquierda
    }
    public void Destruirse()
    {
        gameObject.layer = 2;
        isDying = true;//Se está muriendo
        rb2D.velocity = Vector2.zero;//Se anulan todas las fuerzas del rigidbody
        mechanim.SetTrigger("Destroy");//Se activa el trigger de destruir
        AnimatorStateInfo stateInfo = mechanim.GetCurrentAnimatorStateInfo(0);//Se recibe la animación actual
        float duration = stateInfo.length;//Se recibe la duración de la animación actual
        StartCoroutine(DestruirDespuesDe(duration));//Se ejecuta un ienumerator
    }
    IEnumerator DestruirDespuesDe(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);//espera el tiempo indicado
        EnemySpawner1.EnemigosActuales--;//resta 1 a los enemigos totales creados
        HUDManager.puntuacionActual += 200;//Aumenta la puntuación total
        puntuacion.GetComponent<TextMesh>().text = 200.ToString();//Cambia el texto de puntuación a instanciar
        GameObject clon = Instantiate(puntuacion);//Lo instancia
        clon.transform.position = transform.position;//Lo pone en la misma posición
        GameController.enemigosActuales--;//Reduce el número de enemigos de tipo 1 en la escena al mismo tiempo
        Destroy(gameObject);//Se destruye el objeto
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BalaJugador") && mechanim != null)
        {
            Destruirse();//Si colisiona con las balas del jugador, también se elimina
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            mj1.RestarVida();//Si el trigger de ataque entra en contacto con el jugador, se resta vida al jugador
        }
    }
}