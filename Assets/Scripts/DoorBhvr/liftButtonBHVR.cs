using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class liftButtonBHVR : MonoBehaviour
{
    [SerializeField]
    int sceneToLoadIndex;


    GameObject elevatorDoors;
    bool doorOpen;
    public bool loadsScene = true;
    bool rumble = false;
    bool openAgain = false;
    

     void Start()
    {
        elevatorDoors = GameObject.FindGameObjectWithTag("eDoors");
    }

     void Update()
     {
       // waitForDoorClosed();
     }

    public void handlePress()
    {
        if(doorOpen)
        {
            closeDoors();
            changeScene();
        }
        else
        {
            openDoors();
        }
    }

    void changeScene()
    {
        if(loadsScene)
        {
            SceneManager.LoadScene(sceneToLoadIndex);
        }
        
    }

    void animateButton()
    {
        GetComponent<Animator>().SetTrigger("tElevatorButtonPressed");
    }

    void openDoors()
    {
        elevatorDoors.GetComponent<Animator>().SetBool("isOpen", true);
        elevatorDoors.GetComponent<Animator>().SetTrigger("tAnimEDoor");
        doorOpen = true;
    }

    void closeDoors()
    {
        elevatorDoors.GetComponent<Animator>().SetBool("isOpen", false);
        elevatorDoors.GetComponent<Animator>().SetTrigger("tAnimEDoor");
        doorOpen = false;       
    }

    void waitForDoorClosed()
    {
        if (elevatorDoors.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle-Closed-After"))
        {
            print("doors closed after");
            
        }
    }








}
