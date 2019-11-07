using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{

    public List<GameObject> floors;
    public List<GameObject> walls;
    public List<GameObject> food;
    public List<GameObject> enemies;
    public List<GameObject> outerwalls;
    public GameObject exit;
    public GameObject player;   


    public int columns = 8;
    public int rows = 8;
    //aquí guardamos las posiciones válidas del tablero, para recorrerlas usaremos dos bucles (for), inicializamos la lista = new list<>()
    [SerializeField]
    private List<Vector2> gridPositions = new List<Vector2>();
    [SerializeField]
    private List<Vector2> boardPositions = new List<Vector2>();
    //creamos esta variable para meter los objetos instanciados en ella dentro de la jerarquía y que quede más limpia
    private GameObject _board;


    //creamos una funcion para 
    void InitializeGrid()
    {
        //para cada valor de x necesitamos un o de y
        //por eso hacemos un bucle dentro de otro, para cada valor de x=0 hay 8 valores posibles de y
        //ponemos columns y rows -1 para dejar un pasillo libre para que el jugador se pueda mover y no quede atrapado, por eso también x e y = 1
        for (int x = 1; x < columns-1; x++)
        {
            for (int y = 1; y < rows-1; y++)
            {
                //creamos un vector2 que almacene las posiciones del grid para guardarlas en la lista GridPositions
                Vector2 position = new Vector2(x, y);
                //añadimos esas posiciones a la lista
                gridPositions.Add(position);
                //este debug muestra las coordenadas del grid (0,0), (0,1),...
               //Debug.Log("(" + x + "," + y + ")");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// esta función es igual a la de InitializeGrid pero desde -1 a 8, por eso ponemos columns+1
    /// </summary>
    void BoardSetup()
    {
        for (int x = -1; x < columns+1; x++)
        {
            for (int y = -1; y < rows+1; y++)
            {
                GameObject toInstantiate;
                //creamos vble para guardar la posición en la que instanciar
                Vector2 currentPosition = new Vector2(x, y);
                //ponemos el if para ver cua´ndo ponemos muro -> x = -1 u 8 e y=-1u8
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    //creamos vble para el num aleatorio que elegirá el muro a instanciar
                    int index = Random.Range(0, outerwalls.Count);
                    //creamos la vble que usa el index para sacar el muro que vamos a instanciar, creada más arriba
                    toInstantiate = outerwalls[index];
                }
                //en el else cuando no sea x=-1 y 8 ni y= -1 y 8
                else
                {
                    int index = Random.Range(0, floors.Count);
                    toInstantiate = floors[index];
                }
                //hay que indicar la rotación quaternion.identity; al indicar _board.transform decimos que los instancie en el transform del que será su padre
                Instantiate(toInstantiate, currentPosition, Quaternion.identity, _board.transform);
              
                
                //creamos un vector2 que almacene las posiciones del grid para guardarlas en la lista GridPositions
                Vector2 boardpos = new Vector2(x, y);
                //añadimos esas posiciones a la lista
                boardPositions.Add(boardpos);
            }
        }
    }

    //creamos esta función para inicializar la variable padre de las instancias y limpiar el Start
    //lo ponemos publico para poder llamarlo desde el gamemanager
   public void SceneSetup(int level)
    {
        InitializeGrid();
        //inicializamos la variable para contener los objetos instanciados
        _board = new GameObject();
        _board.name = "Board";
        BoardSetup();
        InstantiateObjectAtRandom(walls, 3, 10);
        InstantiateObjectAtRandom(food, 1, 3);
        InstantiateObjectAtRandom(enemies, level, level);

        Vector2 exitPosition = new Vector2(columns-1, rows-1);
        Instantiate(exit, exitPosition, Quaternion.identity, _board.transform);

        Vector2 playerPosition = new Vector2(0, 0);
        Instantiate (player, playerPosition, Quaternion.identity, _board.transform);


    }

    //para instanciar un objeto aleatorio en una posición aleatoria
    //función que nos devuelve una posición aleatoria (vector2)
    Vector2 GetRandomPos()
    {
        //creamos una vble para numerar los objetos de la lista gridPositions
        int index = Random.Range(0, gridPositions.Count);
        //creamos la vble que almacena la posición aleatoria
        Vector2 posaleat = gridPositions[index];
        //para que no repita ningún valor, sacamos de la lista el valor que haya salido
        gridPositions.RemoveAt(index);
        return posaleat;
    }
    //funcion para instanciar objetos aleatorios en cantidad aleatoria dentro de un minimo y un maximo de objetos
    //llamamos a esta función tantaas veces como listas tenga
    void InstantiateObjectAtRandom(List<GameObject> objectList, int minimum, int maximum)
    {
        
        //elejimos el num de fichas a instanciar
        int index = Random.Range(minimum, maximum+1);
        
        //tenemos que hacer lo mismo apra cada una de las fichas a colocar
        for(int i = 0; i < index; i++)
        {
            //elijo una ficha aleatoriamente
        int index2 = Random.Range(0, objectList.Count);
        //cojo la ficha que he elegido aleatoriamente
         GameObject randomObject = objectList[index2];
         //selecciono una posición aleatoria del tablero para colocar la ficha
         Vector2 randomgridPos = GetRandomPos();
         //coloco la ficha en la posisición elegido
        Instantiate(objectList[index2], randomgridPos, Quaternion.identity, _board.transform);
        }
        
            
    }
}
