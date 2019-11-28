using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public BoardManager boardManager;
    public int playerFood = 100;
    //para saber cuándo es el turno del jugador, cuando esté true será el turno del jugador y cuando false del enemigo
    public bool playerTurn = true;
    //creamos esta bool para que la corrutina no se ejecute indefinidamente
    public bool enemyTurn = false;

    public float enemyDelay = 1;

    private int level = 3;

    //ref al panel
    public GameObject levelImage;
    public Text levelText;

    private void Start()
    {
        InitGame();
    }


    void InitGame()
    {
        //ponemos el turno en falso y en HideLevelImage en true para que no se pueda mover en la pantalla negra de presentación
        playerTurn = false;
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        boardManager.SceneSetup(level);
        Invoke("HideLevelImage", 2);
    }

    void HideLevelImage()
    {
        levelImage.SetActive(false);
        playerTurn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerTurn && enemyTurn)
        {
            enemyTurn = false;
            Debug.Log("Turno de los enemigos");
            StartCoroutine(MoveEnemies());
        }
    }

    protected IEnumerator MoveEnemies()
    {
        //findgameobjectSwithtag devuelve una lista
        //obtenemos lista de enemigos y la metemos en el array enemies creado. Encontramos los nemigos pq tienen el tag Enemy
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //recorremos la lista
        for (int i = 0; i < enemies.Length; i++)
        {
            yield return new WaitForSeconds(enemyDelay);
            //cogemos el script Enemy y llamamos la funcion MoveEnemy
            enemies[i].GetComponent<Enemy>().MoveEnemy();
        }
        playerTurn = true;



    }

    public void NextLevel()
    {
        //buscamos el tablero y lo destruimos, para no construir otro encima
        Destroy(GameObject.Find("Board"));
        level++;
        InitGame();

    }

    public void GameOver()
    {
        levelImage.SetActive(true);
        levelText.text = "After " + level + "days, you starved";
        //desactivamos el script para que no continue el jeugo
        enabled = false;
    }
}
