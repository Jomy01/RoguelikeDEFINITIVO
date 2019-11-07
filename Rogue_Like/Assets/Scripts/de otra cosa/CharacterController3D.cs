using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController3D : MonoBehaviour
{
    //Component Variables
    private Animator myAnimator;
    private NavMeshAgent myNavMeshAgent;
    private FieldOfView myFoV;

    //Movement Variables
    public float crawlingSpeed = 2.5f;
    public float walkingSpeed  = 3.2f;
    public float runningSpeed  = 6.0f;
    public float velocity;

    //Animator Variables
    public bool lying;
    public bool aiming;

    //Input Variables
    public bool leftControl = false;

    Ray cameraRay;
    RaycastHit cameraRayHit;

    //DoubleClick Detector Variables
    private float firstClickTime = 0f;
    private float timeBetweenClicks = 0.2f;
    private bool coroutineAllowed = true;
    private int clickCounter = 0;

    private float maxHelath = 100f;
    public float health = 100f;

    void Start()
    {
        //Components Inicialization
        myFoV = GetComponent<FieldOfView>();
        myAnimator = GetComponent<Animator>();
        myNavMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        SpeedSetter();
        Animation();
        InputDetector();

        if (clickCounter == 1 && coroutineAllowed)
        {
            firstClickTime = Time.time;
            StartCoroutine(DoubleClickDetection());
        }
    }

    void SpeedSetter()
    {
        if (myNavMeshAgent.remainingDistance < 0.1f)
        {
            velocity = 0f;
        }
        else
        {
            velocity = myNavMeshAgent.speed;
        }
    }

    void InputDetector()
    {
        //LeftControl Detector
        if (Input.GetKeyDown(KeyCode.LeftControl))
            leftControl = true;

        if (Input.GetKeyUp(KeyCode.LeftControl))
            leftControl = false;

        //Input
        if (leftControl == true && Input.GetMouseButtonUp(1))
        {
            Aim();
        }

        if (leftControl == true && Input.GetMouseButtonUp(0) && aiming == true)
        {
            Shoot();
        }
        else if (leftControl == false && Input.GetMouseButtonUp(0))
        {
            myNavMeshAgent.acceleration = 0f;
            Walk();
            clickCounter += 1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            LyeDown();
        }            
    }

    private IEnumerator DoubleClickDetection()
    {
        coroutineAllowed = false;

        while (Time.time < firstClickTime + timeBetweenClicks)
        {
            if (clickCounter == 2 && lying == false)
            {
                Run();
                break;
            }

            yield return new WaitForEndOfFrame();
        }

        clickCounter = 0;
        firstClickTime = 0f;
        coroutineAllowed = true;
    }

    void Walk()
    {
        aiming = false;

        if (Physics.Raycast(cameraRay, out cameraRayHit, 100))
        {
            myNavMeshAgent.acceleration = 10f;

            if (lying == false)
            {
                myNavMeshAgent.speed = walkingSpeed;
            }
            else
            {
                myNavMeshAgent.speed = crawlingSpeed;
            }

            myNavMeshAgent.destination = cameraRayHit.point;
        }
    }

    void Run()
    {
        if (Physics.Raycast(cameraRay, out cameraRayHit, 100))
        {
            myNavMeshAgent.acceleration = 10f;
            myNavMeshAgent.speed = runningSpeed;
            myNavMeshAgent.destination = cameraRayHit.point;
        }
    }

    void LyeDown()
    {
        myNavMeshAgent.destination = transform.position;

        if (lying == true)
        {
            lying = false;
        }
        else
        {
            lying = true;
        }
    }

    void Aim()
    {
        if (aiming == false)
        {
            aiming = true;

            if (Physics.Raycast(cameraRay, out cameraRayHit, 100))
            {
                transform.LookAt(cameraRayHit.point);
            }
        }
        else
        {
            if (Physics.Raycast(cameraRay, out cameraRayHit, 100))
            {
                transform.LookAt(cameraRayHit.point);
            }
        }
    }

    void Shoot()
    {
        myAnimator.SetTrigger("Shoot");
    }

    void Animation()
    {
        myAnimator.SetFloat("Velocity", velocity);
        myAnimator.SetBool("Lying", lying);
        myAnimator.SetBool("Aiming", aiming);        
    }
}