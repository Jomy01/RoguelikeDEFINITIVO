using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //necesitaremos un avble para guardar el GameManager
    GameManager gameManager;
    //necesitamos al player, y será el target de los enemigos, irán hacia él
    GameObject target;
    //necesitamos el animator del enemigo
    Animator animator;
    //necesitamos el collider para activarlo y desactivarlo al lanzar el raycast
    BoxCollider2D colaider;

    // Start is called before the first frame update
    void Start()
    {
        //obtenemos el GamManager, su script
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        //en la variable targert metemos el GameObject Player
        target = GameObject.FindGameObjectWithTag("Player");
        //cogemos el Animator de cada enemigo, cada uno coge el suyo ya que cada uno llevará este script y no tenemos que buscar en la escena
        animator = GetComponent<Animator>();
        colaider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
