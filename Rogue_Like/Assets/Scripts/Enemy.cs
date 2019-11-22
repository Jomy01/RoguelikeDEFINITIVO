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

    public float speed = 5;
    public LayerMask mascararaycast;

    //el daño que quita el enemigo
    public int damage = 5;

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

    ///copiamos y pegamos partes del script del Player
    ///--------------------------------------------------------------------------------------------------------


    

    private void IntentarMoverme(float x, float y)
    {
        
       if (IsPossibleMove(x, y))
         {
           Debug.Log("Enemigo se mueve");
           Moverme(x, y);
         }
       else
        {
          Debug.Log("Enemigo no se mueve");
          NomepuedoMover(x, y);
        }
           
    }


    

    
    bool IsPossibleMove(float xDirection, float yDirection)
    {
        //creamos la vable que indicará si nos podemos mover
        bool isPossibleMove = true;


        //guardamos la posición del player en un vector2
        Vector2 currentposition = transform.position;
        //creamos un vector2 con la dirección que toma el player(arriba, abajo, izda o dcha) con las variables de la funcion [(1,0), (0,1), (-1,0),(0,-1)
        Vector2 direction = new Vector2(xDirection, yDirection);

        //desactivamos el collider para que no colisione el rayo con el collider del personaje
        colaider.enabled = false;

        //para lanzar el raycast creamos la variable y luego comprobamos si tiene algo o no, el rayo tiene una distancia de 1
        RaycastHit2D rayo = Physics2D.Raycast(currentposition, direction, 1, mascararaycast);
        Debug.DrawRay(currentposition, direction, Color.yellow);
        //activamos el collider
        colaider.enabled = true;

        //para saber si el rayo ha chocado con algo, el raycastHit guarda un transform que nos indica con qué ha chocado
        //si chocamos no nos podemos mover
        if (rayo.transform)//ha chocado contra algo que tiene el layermask indicado, dentro del radio activado de 1
        {
            isPossibleMove = false;
        }

        return isPossibleMove;
    }

    //funcion para atacar
    void HitPlayer()
    {
        animator.SetTrigger("ataque");
        //para atacar al Player creamos una funcion en el Player controller para quitarle vida (food)
        //ya tenemos el targer buscado en el start, cogemos su script y llamamos a la función Hit y le decimos que le quite damage unidades de comida
        target.GetComponent<PlayerController>().Hit(damage);
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
            if (rayo.transform.CompareTag("Player"))
            {
                HitPlayer();             

            }
        }
    }
    //lo dejamos igual a lo escrito en el PLAYER
    void Moverme(float xDirection, float yDirection)
    {
        //posición en la que estoy
        Vector2 currentposition = transform.position;
        //posicion final,a la que voy=donde estoy más donde quiero ir
        Vector2 posicionfinal = new Vector2(xDirection, yDirection) + currentposition;

        Debug.Log("Moviendo enemigo a la posisición: " + posicionfinal);
        //llamamos a la corrutina
        StartCoroutine(SmoothMovement(posicionfinal));

    }
    //Una corrutina es algo que se ejecuta de forma concurrente, se ejecuta primero el código princiapl, salta a la corrutina y vuelve, de esta manera
    //no tiene que espera el código princiapl a que termine la corrutina (si un personaje tarda un segundo en desplazarse, el código no puede dejar de 
    //ejecutarse ese tiempo, usamos una corrutina para el movimiento que se ejecuta, vuelve al principal y sigue moviendose
    //protected IEnumerator indica que es un corrutina

    //lo dejamos igual a lo escrito en el PLAYER
    protected IEnumerator SmoothMovement(Vector2 posicionfinal) //le pasamos a dónde queremos movernos
    {
        //la funcion MoveTowards desplaza el sprite poco a poco para evitar el teletransporte
        //hacemos un bucle while y en cada iteración me acerco al objetivo, tenemos que hacer el bucle while hasta que casi hemos llegado, ya que la 
        //funciónMoveTowards no llega exactamente al destino, tiene un pequeño error de cálculo de 0,0000algo
        Vector2 remainingDistance = posicionfinal - (Vector2)transform.position;//convertimos transform.position de vector3 a vector2 para poder operar, sería lo mismo new vector2(transform.position.x, transform.position.y)
        //magnitude calcula la distancia de un vector, su hipotenusa
        float remainingDistanceLenght = remainingDistance.magnitude;
        //ponemos le bucle while-> while (condición), cuando deje de cumplirse la condición salimos del bucle
        while (remainingDistanceLenght > Mathf.Epsilon)// Mathf.Epsilon es un valor muy pequeño, menor que 0.01 y que ya viene en Unity)
        {
            //la siguiente posición a la que voy es la indicada por el vector resultante de ir del inicio al final a la velocidad*timedeltatime para que no afecte la velocidad de frames
            Vector2 nextPosition = Vector2.MoveTowards(transform.position, posicionfinal, speed * Time.deltaTime);
            transform.position = nextPosition;//actualizamos la posición inicial para seguir moviéndonos
            //actualizamos la condicion haciendo una única línea de las líneas de arriba Vector2 remainingDistance y float remainingDistanceLenght
            remainingDistanceLenght = (posicionfinal - (Vector2)transform.position).magnitude;


            //devuelve el control al código principal, de esta manera se ejecuta la corrutina, vuleve al código principal y entra a la corrutina hasta que acabe el bucle while que hemos puesto, una corrutina puede hacer que algo espere x segundo, por ejemplo
            yield return null;
        }


    }

    //funcion para que se mueva el enemigo
    public void MoveEnemy()
    {
        //declaramos la posición en x e y del jugador y enemigo
        float enemX = transform.position.x;
        float enemY = transform.position.y;

        float playerX = target.transform.position.x;
        float playerY = target.transform.position.y;
        //declaramos la distancia en x de las variables anteriores en valor absoluto para que no hay avalores negativos
        float distX = Mathf.Abs( playerX - enemX);
        float distY = Mathf.Abs(playerY - enemY);
        //si la distancia en x es menor que en Y nos movemos en x
        if (distX < distY)
        {
            Debug.Log("Calculando distancia");
            //si el player está más a la izda nos movemos a la izda
            if (playerX < enemX)
            {
                Moverme(-1, 0);
                //IntentarMoverme(-1, 0);
            }
            else
            {
                Moverme(1, 0);
                //IntentarMoverme(1, 0);
            }
        }
        //si la dist en x es mayor que en Y nos movemos en Y
        else
        {
            //si el player está más abajo nos movemos abajo
            if (playerY < enemY)
            {
                Moverme(0, -1);
                //IntentarMoverme(0, -1);
            }
            else
            {
                Moverme(0, 1);
                //IntentarMoverme(0, 1);
            }

        }




       // otro método para acercarse al player, mediante decisiones aleatorias
         
        /*void MoveEnemy()
        {
        Vector2 posicionEnemigo = transform.position;
        Vector2 posicionPlayer = target.transform.position;

        Vector2 distancia = posicionPlayer - posicionEnemigo;
        // calcula las 4 posiciones posibles de movimiento del enemigo, para saber qué movimiento favorece más
        Vector2 movDerecha = new Vector2(posicion.x + 1, posicion.y);
        Vector2 movIzda = new Vector2(posicion.x - 1, posicion.y);
        Vector2 movArriba = new Vector2(posicion.x , posicion.y+ 1);
        Vector2 movDerecha = new Vector2(posicion.x, posicion.y - 1);

        bool derecha = false;
        bool arriba = false;

        int aleatorio = Random.Range (0,2);

        //comprobamos si la distancia es mayor que la pos del player - mover a ladcha en valor absoluto, si se cumple -> derecha = true
        if(distancia.magnitude > (posicionPlayer - movDerecha).magnitude)
        {
        derecha = true;
        }

        else derecha = false;


        if(distancia.magnitude > (posicionPlayer - movArriba).magnitude)
        {
        arriba = true;
        }

        else arriba = false;

        if(aleatorio == 0) //x
        {
            if (derecha == true)
            {
            posicionDecidida = new Vector2 (1,0);
            }
            else 
            {
            posicionDecidida = new Vector2 (-1,0);
            }
        }

        else    //y
        {
        if (arriba == true)
        {
            posicionDecidida = new Vector2 (0, 1)
        }

        else
        {
        posicionDecidida = new Vector2 (0,-1)
        }

        }

    */
    
       // IntentarMoverme(posicionDecidida.x, posicionDecidida.y);
          
         



    }

}
