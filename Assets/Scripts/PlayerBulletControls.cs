using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Bala usada en los controles
public class PlayerBulletControls : MonoBehaviour
{
    Rigidbody2D rb2D;//Recibe el rigidbody
    [SerializeField]
    Sprite[] sprites;//Array con los sprites
    Image image;//Componente imagen
    [SerializeField]
    float speed;//velocidad de movimiento
    [SerializeField]
    float timer;//Temporizador antes de que sea destruido
    PlayerControls pc1;
    void Start()
    {
        pc1 = FindObjectOfType<PlayerControls>();//Busca en escena el objeto con este script
        image = GetComponent<Image>();//Recibe el componente imagen del objeto
        rb2D = GetComponent<Rigidbody2D>();//Recibe el rigidbody del objeto
        rb2D.AddForce(Vector2.up * speed, ForceMode2D.Impulse);//Añade una fuerza con la velocidad hacia arriba
        //El sprite que renderiza la imagen es generado aleatoriamente dentro del array de sprites
        image.sprite = sprites[Random.Range(0, sprites.Length)];
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (timer > 0)//Si el temporizador es mayor que 0
        {
            timer -= Time.fixedDeltaTime;//Continúa
        }
        if (timer <= 0)//Si es menor o igual de 0
        {
            Destroy(gameObject);//Se destruye
            pc1.frutaSpawneada = false;//Y frutaSpawneada se vuelve falso
        }
    }
}
