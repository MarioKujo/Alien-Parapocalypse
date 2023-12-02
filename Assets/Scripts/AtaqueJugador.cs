using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AtaqueJugador : MonoBehaviour
{
    [SerializeField]
    GameObject groundAttack;//Revisa donde poner la hitbox de ataque
    [SerializeField]
    Transform airAttack;//De donde sale el ataque aéreo
    [SerializeField]
    GameObject Bala;//Para poder spawnear balas
    [SerializeField]
    float Temporizador;//Tiempo fijo entre ataques
    [SerializeField]
    float attackRange;//Rango de ataque cuerpo a cuerpo
    [SerializeField]
    LayerMask enemyLayer;//Máscara de capa para enemigos
    float AtaqueSuelo;//Tiempo antes de que puedas atacar de nuevo
    float AtaqueAire;//Tiempo antes de que puedas lanzar una fruta de nuevo
    Animator mechanim;//Animador
    private void Awake()
    {
        groundAttack.SetActive(false);//Desactiva el gameobject de ataque
        mechanim = GetComponent<Animator>();//Recibe el componente del objeto
    }
    private void FixedUpdate()
    {
        if (AtaqueSuelo > 0)
        {
            AtaqueSuelo -= Time.fixedDeltaTime;//Si el temporizador está corriendo, sigue
        }
        if (AtaqueAire > 0)
        {
            AtaqueAire -= Time.fixedDeltaTime;//Si el temporizador está corriendo, sigue
        }
        if (Input.GetKey(KeyCode.P) && AtaqueSuelo <= 0)//Si se pulsa el botón de ataque y el temporizador ha llegado a 0
        {
            StartCoroutine(GroundAttack());//Hace la función de ataque
            AtaqueSuelo = Temporizador;//Vuelve a iniciar el temporizador
        }
        if (Input.GetKey(KeyCode.O) && AtaqueAire <= 0)
        {
            AirAttack();//Si se pulsa la tecla y el temporizador ha acabado, puede volver a lanzar una fruta
        }
    }
    void AirAttack()//Ataque aéreo
    {
        GameObject clon = Instantiate(Bala);//Instancia la bala
        clon.transform.position = airAttack.position;//En la posición indicada
        AtaqueAire = Temporizador;//Y vuelve a empezar el temporizador
    }
    IEnumerator GroundAttack()//Ataque en el suelo
    {
        mechanim.SetTrigger("Attack");//Activa el trigger del animador
        AnimatorStateInfo stateInfo = mechanim.GetCurrentAnimatorStateInfo(0);//Recibe la información de la animación
        float duration = stateInfo.length;//Introduce la duración de la animación actual en un float
        groundAttack.SetActive(true);//Activa el gameobject de ataque
        yield return new WaitForSeconds(duration);//Y espera los segundos que dura la animación
        groundAttack.SetActive(false);//Y lo desactiva
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        MovimientoEnemigo1[] me1 = FindObjectsOfType<MovimientoEnemigo1>();//Array que recibe todos los enemigos 1
        MovimientoEnemigo2[] me2 = FindObjectsOfType<MovimientoEnemigo2>();//Array que recibe todos los enemigos 2
        for (int i = 0; i < me1.Length; i++)//Empieza un bucle para revisar todos los enemigos en la escena
        {
            //Si colisiona con un enemigo de tipo 1 y coincide que la colisión del ataque es igual que la del enemigo en el array
            if (collision.CompareTag("Enemigo1") && collision.gameObject == me1[i].gameObject)
            {
                me1[i].Destruirse();//Se destruye el enemigo mediante su función
            }
        }
        for (int i = 0; i < me2.Length; i++)
        {
            //Si colisiona con un enemigo de tipo 2 y coincide que la colisión del ataque es igual que la del enemigo en el array
            if (collision.CompareTag("Enemigo2") && collision.gameObject == me2[i].gameObject)
            {
                me2[i].Destruirse();//Se destruye el enemigo mediante su función
            }
        }
    }
}