using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MovimientoJugador : MonoBehaviour
{
    public int vida;//Vida del jugador al 100%
    public float moveSpeed = 1.5f;//Velocidad de movimiento (se puede cambiar en el editor)
    [SerializeField]
    float jumpSpeed = 1.5f;//Velocidad de salto (se puede cambiar en el editor)
    [SerializeField]
    float temporizadorInvencible;//Temporizador que indica cuanto tiempo parpadea y cuanto tiempo es invencible
    [SerializeField]
    Transform Grounded;//Posici�n desde la que lanzar un rayo hacia abajo para revisar si est� en el suelo
    [SerializeField]
    LayerMask isGround;//M�scara de capa (suelo)
    bool grounded;//Booleano para revisar si est� tocando el suelo
    Rigidbody2D rb;//Rigidbody para velocidad
    SoundManager soundManager;//Hacer que suene
    public static int vidaActual;//vida actual del personaje
    public static bool isRight = true;//Si est� mirando hacia la derecha
    float x;//Movimiento en el eje x
    Animator mechanim;//Animador
    SpriteRenderer sr;//Renderizador de sprites
    GameController gc;//Controlador del juego
    bool playing = false;//Si el sonido se est� reproduciendo
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();//Recibe el componente de renderizar
        rb = GetComponent<Rigidbody2D>();//rb recibe las operaciones de rigidbody
        mechanim = GetComponent<Animator>();//Recibe el animador
        soundManager = FindObjectOfType<SoundManager>();//Busca el objeto con el script en la escena
        vidaActual = vida;//La vida actual es la m�xima
        gc = FindObjectOfType<GameController>();//Busca el objeto con el script en la escena
    }
    private void Update()
    {
        if (vidaActual <= 0 && !gc.isGameOver)//Si la vida actual es mayor o igual a 0 y todav�a no ha perdido
        {
            if (HUDManager.puntuacionActual > HUDManager.Record)//Si la puntuaci�n actual es mayor que el r�cord
            {
                HUDManager.Record = HUDManager.puntuacionActual;//La puntuaci�n actual se vuelve el r�cord
                HUDManager.newRecord = true;//Y el booleano se vuelve verdadero tambi�n
            }
            gc.GameOver();//Se ejecuta una funci�n de otro script
        }
        else if (vidaActual != 0)//Si la vida actual no es 0
        {
            Move();//Funci�n para moverse
            IsGrounded();//Funci�n que revisa si puedes saltar
            Jump();//Funci�n para saltar
        }
    }
    void Move()
    {
        x = Input.GetAxis("Horizontal");//Recibe el eje horizontal
        mechanim.SetFloat("Move",Mathf.Abs(x));//Se mueve
        rb.velocity = new Vector2(x * moveSpeed, rb.velocity.y);//Cambia la velocidad del rigidbody para moverse
        if (x > 0)//Si se mueve hacia la derecha
        {
            isRight = true;//Est� mirando hacia la derecha
            transform.eulerAngles = Vector3.zero;//Se gira hacia la derecha
        }
        if (x < 0)//Si se mueve hacia la izquierda
        {
            isRight = false;//Est� mirando hacia la izquierda
            transform.eulerAngles = new Vector3(0, 180, 0);//Se gira hacia la izquierda
        }
        if (x != 0 && !playing)//Si se est� moviendo y el sonido no se est� reproduciendo actualmente
        {
            StartCoroutine(playSound(soundManager.clipLength(5)));//Empieza una corutina para reproducir el sonido
        }
    }
    IEnumerator playSound(float time)
    {
        playing = true;//Se est� reproduciendo el sonido
        soundManager.SeleccionAudio(5, 1f);//Se reproduce con un volumen de 0.2
        yield return new WaitForSeconds(time);//Espera el tiempo de reproducci�n
        playing = false;//Se deja de reproducir el sonido
    }
    void IsGrounded()
    {
        //Tira un rayo al suelo con m�scara de capa para que s�lo note el suelo
        RaycastHit2D hit = Physics2D.Raycast(Grounded.transform.position, Vector2.down, 0.5f, isGround);
        Debug.DrawRay(Grounded.transform.position, Vector2.down * 0.5f, Color.blue);//Dibuja un rayo rojo hacia abajo
        grounded = false;//Empieza en falso
        if(hit)
        {
            if (hit.collider.CompareTag("Suelo"))//Si le da al suelo
            {
                grounded = true;//Est� en el suelo
            }
        }
    }
    void Jump()
    {
        //Si est� en el suelo y toca "Up" (ya sea w, flecha hacia arriba o el bot�n del mando), salta
        if (Input.GetButtonDown("Up") && grounded)
        {
            rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);//Impulsa el personaje hacia arriba con la velocidad especificada
            mechanim.SetTrigger("Jump");
        }
    }
    public void RestarVida()//Resta vida
    {
        vidaActual--;
        StartCoroutine(Blink());//Empieza una corutina para volver al personaje invencible por un tiempo
    }
    IEnumerator Blink()
    {
        gameObject.layer = 2;//Ignora las colisiones de los ataques enemigos
        for (float i = 0; i < temporizadorInvencible; i += Time.fixedDeltaTime)//Bucle que se repite 6 veces
        {
            sr.color = new Color(1f, 1f, 1f, 0f);//Se vuelve invisible
            yield return new WaitForSeconds(0.1f);//espera una d�cima de segundo
            sr.color = new Color(1f, 1f, 1f, 1f);//Se vuelve visible
            yield return new WaitForSeconds(0.1f);//Espera otra d�cima
        }
        gameObject.layer = 8;//Y vuelve a activar las colisiones
    }
}