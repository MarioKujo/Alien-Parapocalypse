using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet2 : MonoBehaviour
{
    [SerializeField]
    float velocidadBala;//Velocidad de la bala
    Vector3 direccionDisparo;//Direcci�n del disparo
    Rigidbody2D rb2D;//Referencia al rigidbody
    MovimientoJugador mj1;
    Animator mechanim;
    public bool isDying;//Referencia a si est� muriendo o no
    void Awake()
    {
        mechanim = GetComponent<Animator>();//Recibe el componente del animador
        mj1 = FindObjectOfType<MovimientoJugador>();//Busca en escena el componente con este script
        rb2D = GetComponent<Rigidbody2D>();//Recibe el rigidbody de su objeto
    }
    void FixedUpdate()
    {
        if (!isDying)
        {
            rb2D.velocity = direccionDisparo.normalized * velocidadBala;//Si no se est� muriendo se mueve en direcci�n al personaje
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si colisiona con el jugador, con el suelo o con la bala del jugador
        if (collision.CompareTag("Player") || collision.CompareTag("Suelo") || collision.CompareTag("BalaJugador"))
        {
            Destruirse(collision);//Se destruye
        }
    }
    public void Inicializar(Vector3 direccion)
    {
        direccionDisparo = direccion;//Se inicializa con la direcci�n que se le pasa
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);//Se destruye sin animaci�n al salir de la pantalla
    }
    public void Destruirse(Collider2D collider)//Funci�n para destruirse
    {
        rb2D.bodyType = RigidbodyType2D.Static;//El rigidbody se vuelve est�tico
        if (collider.CompareTag("Player"))
        {
            mj1.RestarVida();//Si colisiona con el jugador, se le resta vida
        }
        isDying = true;//Se est� muriendo
        mechanim.SetTrigger("Explode");//Salta el trigger de explosi�n
        AnimatorStateInfo stateInfo = mechanim.GetCurrentAnimatorStateInfo(0);//Recibe el estado de la animaci�n actual
        float duration = stateInfo.length;//Recibe la duraci�n de la animaci�n actual
        StartCoroutine(DestruirDespuesDe(duration));//Empieza una corutina para que se destruya despu�s de la animaci�n
    }
    IEnumerator DestruirDespuesDe(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);//Espera el tiempo de animaci�n
        Destroy(gameObject);//Y se destruye
    }
}