using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerBullet : MonoBehaviour
{
    [SerializeField]
    float BulletSpeed;//Velocidad de la bala
    [SerializeField]
    Sprite[] spritesBalas;//Array para introducir todos los sprites a usar
    SpriteRenderer sr;//Para renderizar esos sprites
    Rigidbody2D rb;//Rigidbody
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();//Recibe el rigidbody de la bala
        sr = GetComponent<SpriteRenderer>();//Recibe el spriteRenderer de la bala
        sr.sprite = spritesBalas[Random.Range(0, spritesBalas.Length)];//Al inicializarse, selecciona uno de los sprites aleatoriamente para renderizarlo
        rb.AddForce(transform.up * BulletSpeed, ForceMode2D.Impulse);//Al inicializarse, aplica fuerza de impulso al rigidbody
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Si la colisión es cualquiera de los 3 enemigos o una bala de un enemigo
        if (collision.gameObject.CompareTag("Enemigo1") || collision.gameObject.CompareTag("Enemigo2") || collision.gameObject.CompareTag("Enemigo3") || collision.gameObject.CompareTag("BalaEnemigo"))
        {
            Destroy(gameObject);//Se destruye
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);//Si se sale de la escena, se destruye
    }
}