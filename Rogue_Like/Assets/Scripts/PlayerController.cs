using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int PointsPerSoda = 20;
    //para acceder al GameManager.cs para acceder a los puntos de comida
     public GameManager gameManager;
    //para arrastrar el gamemanager directamente
    //public GameObject gameManager;
    //gameManager = GameObject.FindWithTag ("GameController");

    private int currentFoodPoints;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        currentFoodPoints = gameManager.playerFood;//carga los putnos de comida iniciales
        animator = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.playerTurn)
        {
            //intento moverme
            IntentarMoverme();
        }
    }

    void ComprobarSiGameOver()
    {
        if (currentFoodPoints < 1) gameManager.GameOver();
    }

    private void IntentarMoverme()
    {
        //usamos getaxisRaw pq no queremos decimales
        //nos quedaremos solo con mov vertical u horizontal, uno de los dos
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //si nos movemos en x no nos movemos en y; cuando no nos movemos en x lo hacemos en y; cuando hay una unica linea, podemos ponerlo         if (x!= 0)  y = 0;
        if (x!= 0)
        {
            y = 0;
        }
        else
        {
            x = 0;
        }
        //si me muevo
        if ( x != 0 || y != 0)
        {
            currentFoodPoints--;
            //comprobar si me puedo mover, si puedo, me muevo; tal vez pueda destruir un muro
            ComprobarSiGameOver();
            gameManager.playerTurn = false;
        }


    }
}
