using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ejercicios : MonoBehaviour
{

    
    // Start is called before the first frame update
    void Start()
    {
        
        print(HelloWorld("Hello World!"));
        print(Ejercicio2(2));
        print(Ejercicio3("Julio"));
        print(Sum(3f, 4f));
        print(Multiply(2f, 3f, 4f));
        print(Aleatorio(0));
        Lista(5);
        Lista2(2);



    }

    // Update is called once per frame
    void Update()
    {

    }
    //ejercicio 1
    string HelloWorld(string message)
    {
        return message;
    }

    float Ejercicio2(float numero)
    {
        return numero;
    }

    string Ejercicio3(string nombre)
    {
        return nombre;
    }
    //ejercicio 4
    float Sum(float num1, float num2)
    {
        float suma = num1 + num2;
        return suma;
    }
    //ejercicio 5
    float Multiply(float num3, float num4, float num5)
    {
        float multipl = num3 * num4 * num5;
        return multipl;
    }
    //ejercicio 6
    float Aleatorio(float num6)
    {
        float aleat = Random.Range(-20f, num6 + 1);
        return aleat;
    }
    //ejercicio 7
    List<int> Lista(int num7)
    {
        //inicializamos la lista
        List<int> listilla = new List<int>();

        for (int i = 0; i < num7; i++)
        {
            listilla.Add(i);
        }
        for (int i = 0; i < listilla.Count; i++)
        {
            Debug.Log(listilla[i]);
        }

        return listilla;

    }


    //ejercicio 8
    List<int> Lista2(int num7)
    {
        //inicializamos la lista
        List<int> listaParametros = new List<int>();
        List<int> listilla = new List<int>();

        for (int j = 0; j < 100; j++)
        {
        listaParametros.Add(j);

        }


        for (int i = 0; i < num7; i++)
        {
            listilla.Add(i);
        }
        for (int i = 0; i < listilla.Count; i++)
        {
            Debug.Log(listilla[i]);
        }

        return listilla;

    }

    //ejercicio 9
    //soy incapaz de sacarlo

    

 
}