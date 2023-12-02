using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyBullet3 : MonoBehaviour
{
    Rigidbody2D rb2D;//Rigidbody de la bala
    MovimientoJugador mj1;//recibe los componentes del jugador
    Animator mechanim;//Animador
    int playerHit = 0;//N�mero de veces que pega al jugador
    bool destroyed = false;
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();//Recibe una referencia a su propio rigidbody
        mechanim = GetComponent<Animator>();//Recibe una referencia a su propio animator
        mj1 = FindObjectOfType<MovimientoJugador>();//recibe una referencia del jugador
    }
    private void FixedUpdate()
    {
        if (destroyed)
        {
            rb2D.velocity = Vector2.zero;//Si se est� destruyendo, se le aplica velocidad 0 todos los frames (para que no vuele hacia arriba)
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si colisiona con el jugador o con la bala del jugador o con el suelo
        if (collision.CompareTag("BalaJugador") || collision.CompareTag("Player") || collision.CompareTag("Suelo"))
        {
            Destruirse(collision);//Se destruye
        }
    }
    //Si sale de la pantalla
    private void OnBecameInvisible()
    {
        Destroy(gameObject);//Se destruye sin animaci�n
    }
    //Funci�n para destruirse (se le pasa un collider porque se usa raycast hit para detectar la colisi�n)
    public void Destruirse(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerHit != 1)//Si la colisi�n es el jugador y todav�a no le ha hecho da�o
        {
            mj1.RestarVida();//Se resta la vida del jugador
            playerHit++;//Y se indica que le ha golpeado
        }
        mechanim.SetTrigger("Explode");//Salta el trigger de explosi�n
        AnimatorStateInfo stateInfo = mechanim.GetCurrentAnimatorStateInfo(0);//Recibe el estado de la animaci�n actual
        float duration = stateInfo.length;//Recibe la duraci�n de la animaci�n actual
        StartCoroutine(DestruirDespuesDe(duration, collision));//Empieza una corutina para que se destruya despu�s de la animaci�n
    }
    IEnumerator DestruirDespuesDe(float tiempo, Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))//Si colisiona con el jugador
        {
            rb2D.velocity = Vector2.down * 30;//Baja al suelo r�pido
        }
        if (collision.gameObject.CompareTag("BalaJugador") || collision.gameObject.CompareTag("Suelo"))//Si colisiona con la bala o con el suelo
        {
            destroyed = true;
            rb2D.gravityScale = 0;
        }
        yield return new WaitForSeconds(tiempo);//Espera el tiempo de la animaci�n
        Destroy(gameObject);//Y se destruye
    }
}
