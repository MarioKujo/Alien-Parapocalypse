using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    [SerializeField]
    float moveSpeed;//Velocidad de movimiento
    AnimatorStateInfo asi1;//Para recibir la información de la animación
    Animator mechanim;//El animador actual
    float duration;//Duración de la animacion
    private void Start()
    {
        mechanim = GetComponent<Animator>();//Recibe el animador
        asi1 = mechanim.GetCurrentAnimatorStateInfo(0);//Recibe la animación actual
        duration = asi1.length;//Recibe la longitud de la animación
        StartCoroutine(DestruirseDespuesDe(duration));//Empieza una corutina para destruirlo
    }
    IEnumerator DestruirseDespuesDe(float tiempo)//Corutina para desturirlo después de acabar su animación
    {
        yield return new WaitForSeconds(tiempo);//Espera el tiempo determinado
        Destroy(gameObject);//Se destruye
    }
    private void FixedUpdate()
    {
        transform.position += transform.up * moveSpeed;//Se mueve hacia arriba mientras existe
    }
}
