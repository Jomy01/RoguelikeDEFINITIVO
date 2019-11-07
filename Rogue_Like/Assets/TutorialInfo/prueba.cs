using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prueba : MonoBehaviour
{

    public int columns = 8;
    public int rows = 8;
    List<Vector2> Positions = new List<Vector2>();
    void Ejercicio10()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                //creamos un vector2 que almacene las posiciones para guardarlas en la lista Positions
                Vector2 position = new Vector2(x, y);
                //añadimos esas posiciones a la lista
                Positions.Add(position);
                //este debug muestra las coordenadas (0,0), (0,1),...
                Debug.Log("(" + x + "," + y + ")");
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
          //sólo sé imprimir todos los valores activando el Debug que hay dentro de Ejercicio10
        Ejercicio10();


          //sólo sé imprimir todos los valores activando el Debug que hay dentro de ejercicio11()
         ejercicio11();
          //imprime el tamaño total
          print("Hay " + ejercicio11().Count + " elementos en la lista");
          //imprimir el cuarto valor
          print(ejercicio11()[4]);
          //borrar
          ejercicio11().Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<Vector2> ejercicio11()
    {
        for (int x = 1; x < columns; x++)
        {
            for (int y = 1; y < rows; y++)
            {
                //creamos un vector2 que almacene las posiciones del grid para guardarlas en la lista GridPositions
                Vector2 position = new Vector2(x, y);
                //añadimos esas posiciones a la lista
                Positions.Add(position);
                
               
            }
        }
        for (int x = 1; x < Positions.Count; x++)
        {
            for (int y = 1; y < Positions.Count; y++)
            {
                //Debug.Log(Positions[x]);
                //Debug.Log(Positions[y]);
            }
        }

        return Positions;
    }

   

}
