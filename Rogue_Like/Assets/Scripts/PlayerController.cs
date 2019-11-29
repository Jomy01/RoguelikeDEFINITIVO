using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int PointsPerSoda = 20;
    //para acceder al GameManager.cs para acceder a los puntos de comida
     GameManager gameManager;
    //para arrastrar el gamemanager directamente
    //public GameObject gameManager;
    //gameManager = GameObject.FindWithTag ("GameController");

    //creamos vable layermask para el raycast
    public LayerMask mascararaycast;

    private int currentFoodPoints;
    private Animator animator;
    //declaramos el collider para poder desactivarlo antes de lanzar el raycast y que no choque consigo mismo, lo activamos después de lanzarlo
    private BoxCollider2D colaider;

    public Text food;

    //audio
   
    public AudioClip[] movClips;
    public AudioClip[] eatClips;
    public AudioClip[] drinkClips;
    public AudioClip gameoverClip;
    private Audio audioController;








    //variable para que no de error al llegar a la salida
    public bool onExit = false;

    // Start is called before the first frame update
    void Start()
    {
        //cargamos el script gamemanager en tiempo de ejecucion
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        currentFoodPoints = gameManager.playerFood;//carga los putnos de comida iniciales
        food.text = "Food: " + currentFoodPoints;
        animator = GetComponent<Animator>();
        colaider = GetComponent<BoxCollider2D>();
        audioController = GameObject.Find("AudioController").GetComponent<Audio>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.playerTurn)
        {
            Debug.Log("Es mi turno");
            onExit = false;
            //intento moverme
            IntentarMoverme();
        }
    }

    void ComprobarSiGameOver()
    {
        if (currentFoodPoints < 1)
        {
            audioController.PlaySingle(gameoverClip);
            audioController.StopMusic();
            gameManager.GameOver();
        }
    }

    private void IntentarMoverme()
    {
        //usamos getaxisRaw pq no queremos decimales, toma valores enteros , -1, 0 o 1
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
            food.text = "Food: " + currentFoodPoints;
            //comprobar si me puedo mover, si puedo, me muevo; tal vez pueda destruir un muro, le pasamos los valores de la x e y de IntentarMoverme, ya que hemos dicho que IsPossibleMove tiene dos variables
           if( IsPossibleMove(x, y))
            {
                Debug.Log("Me puedo mover");
                Moverme(x, y);
            }
            else
            {
                Debug.Log("No me puedo mover");
                NomepuedoMover(x, y);
            }
            ComprobarSiGameOver();
            gameManager.playerTurn = false;
            gameManager.enemyTurn = true;
        }


    }

    //para ver si podemos movernos, usamos un nombredescriptivo y que empiece por "is" ya que nos va a devolver un bool, se podrá mover o no
    //le ponemos dos valores que necesitaremos x e y
    bool IsPossibleMove(float xDirection, float yDirection)
    {
        //creamos la vable que indicará si nos podemos mover
        bool isPossibleMove = true;


        //guardamos la posición del player en un vector2
        Vector2 currentposition = transform.position;
        //creamos un vector2 con la dirección que toma el player(arriba, abajo, izda o dcha) con las variables de la funcion [(1,0), (0,1), (-1,0),(0,-1)
        Vector2 direction = new Vector2 (xDirection, yDirection);

        //desactivamos el collider para que no colisione el rayo con el collider del personaje
        colaider.enabled = false;
        
        //para lanzar el raycast creamos la variable y luego comprobamos si tiene algo o no, el rayo tiene una distancia de 1
        RaycastHit2D rayo = Physics2D.Raycast(currentposition, direction, 1, mascararaycast);
        
        Debug.DrawRay(currentposition, direction, Color.yellow);
        //activamos el collider
        colaider.enabled = true;

        //para saber si el rayo ha chocado con algo, el raycastHit guarda un transform que nos indica con qué ha chocado
        //si chocamos no nos podemos mover
        if(rayo.transform)//ha chocado contra algo que tiene el layermask indicado, dentro del radio activado de 1
        {
            isPossibleMove = false;
        }

        return isPossibleMove;
    }


    void NomepuedoMover(float xDirection, float yDirection)
    {
        //vamos a comprobar si hay un muro destructible, si lo hay, atacamos al muro. Con un linecast
         
        Vector2 currentposition = transform.position;
        Vector2 posicionfinal = new Vector2(xDirection, yDirection) + currentposition;
        colaider.enabled = false;
        RaycastHit2D rayo = Physics2D.Linecast(currentposition, posicionfinal, mascararaycast);
        Debug.DrawLine(currentposition, posicionfinal, Color.blue);
        colaider.enabled = true;
        //rayo almacena el objeto contra el que ha chocado el raycast y esta en layermask
        //si en la vble rayo tenemos algo (ha chocado con algo)
        if (rayo.transform)
        {
            if(rayo.transform.CompareTag("Wall"))
            {
                //el objeto contra el que ha chocado el rayo es:
                // rayo.transform.gameObject;
                //queremos que coja el gameobject con el que ha chocado, tome su script Walls y llame a la función DamageWall
                rayo.transform.gameObject.GetComponent<Walls>().DamageWall(1);
                animator.SetTrigger("attack");
                //creamos funcion para atacar muro
                //AtacarMuro()

            }
        }
    }

    void Moverme(float xDirection, float yDirection)
    {
        //posición en la que estoy
        Vector2 currentposition = transform.position;
        //posicion final,a la que voy=donde estoy más donde quiero ir
        Vector2 posicionfinal = new Vector2(xDirection, yDirection) + currentposition;

        Debug.Log("Moviendo player a la posisición: " + posicionfinal);
        //llamamos a la corrutina
        StartCoroutine(SmoothMovement(posicionfinal));

        audioController.PlayRandomClip(movClips);

    }
        //Una corrutina es algo que se ejecuta de forma concurrente, se ejecuta primero el código princiapl, salta a la corrutina y vuelve, de esta manera
        //no tiene que espera el código princiapl a que termine la corrutina (si un personaje tarda un segundo en desplazarse, el código no puede dejar de 
        //ejecutarse ese tiempo, usamos una corrutina para el movimiento que se ejecuta, vuelve al principal y sigue moviendose
        //protected IEnumerator indica que es un corrutina
    protected IEnumerator SmoothMovement(Vector2 posicionfinal) //le pasamos a dónde queremos movernos
    {
        //la funcion MoveTowards desplaza el sprite poco a poco para evitar el teletransporte
        //hacemos un bucle while y en cada iteración me acerco al objetivo, tenemos que hacer el bucle while hasta que casi hemos llegado, ya que la 
        //funciónMoveTowards no llega exactamente al destino, tiene un pequeño error de cálculo de 0,0000algo
        Vector2 remainingDistance = posicionfinal -(Vector2)transform.position;//convertimos transform.position de vector3 a vector2 para poder operar, sería lo mismo new vector2(transform.position.x, transform.position.y)
        //magnitude calcula la distancia de un vector, su hipotenusa
        float remainingDistanceLenght = remainingDistance.magnitude;
        //ponemos le bucle while-> while (condición), cuando deje de cumplirse la condición salimos del bucle
        while (remainingDistanceLenght > Mathf.Epsilon && !onExit)// Mathf.Epsilon es un valor muy pequeño, menor que 0.01 y que ya viene en Unity)
        {
            //la siguiente posición a la que voy es la indicada por el vector resultante de ir del inicio al final a la velocidad*timedeltatime para que no afecte la velocidad de frames
            Vector2 nextPosition = Vector2.MoveTowards(transform.position, posicionfinal, speed * Time.deltaTime);
            transform.position = nextPosition;//actualizamos la posición inicial para seguir moviéndonos
            //actualizamos la condicion haciendo una única línea de las líneas de arriba Vector2 remainingDistance y float remainingDistanceLenght
            remainingDistanceLenght = (posicionfinal - (Vector2)transform.position).magnitude;


        //devuelve el control al código principal, de esta manera se ejecuta la corrutina, vuleve al código principal y entra a la corrutina hasta que acabe el bucle while que hemos puesto, una corrutina puede hacer que algo espere x segundo, por ejemplo
        yield return null; 
        }

        //funcion para que cuando el enemigo ataque le quite vida, el int es el daño que nos hacen
       
    }


    public void Hit(int losefood)
    {
        //currentfood = current - losefood
        currentFoodPoints -= losefood;
        food.text = "Food: " + currentFoodPoints;
        animator.SetTrigger("Player hitted");
        ComprobarSiGameOver();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("Exit"))
        {
            gameManager.NextLevel();
            transform.position = Vector2.zero;//nos llevamos al player al (0,0) = new Vector2(0,0)
            collision.gameObject.SetActive(false);
            onExit = true;

        }

        else if (collision.transform.CompareTag("Food"))
        {
            currentFoodPoints += pointsPerFood;
            food.text = "Food: " + currentFoodPoints;
            collision.gameObject.SetActive(false);
            audioController.PlayRandomClip(eatClips);
        }

        else if (collision.transform.CompareTag("Soda"))
        {
            currentFoodPoints += PointsPerSoda;
            food.text = "Food: " + currentFoodPoints;
            collision.gameObject.SetActive(false);
            audioController.PlayRandomClip(drinkClips);
        }
    }
}

