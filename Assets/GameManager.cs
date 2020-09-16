using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    //Character & Movement
    public Character GameCharacter;
    public Camera Camera;
    public Vector3 CameraOffset;
    public float mouseSensitivity;
    public bool paused;
    public int pauseint;
    //World Processes

        //store locations
    public Vector3[] SectionLocations;
    public Vector3 CheckoutLocation;
    public int[] SectionTypes;
    public float distanceThreshold;

    public UIMaster UIMasterObject;

    public float availableFunds;
    

    void Start()
    {
        pauseint = 1;   
    }

    // Update is called once per frame
    void Update()
    {
        //  PLAYER INPUT
        if (paused == false)
        {
            if (Input.GetKey(KeyCode.W))
            {
                GameCharacter.MoveForward();
            }
            if (Input.GetKey(KeyCode.A))
            {
                GameCharacter.MoveLeft();
            }
            if (Input.GetKey(KeyCode.S))
            {
                GameCharacter.MoveBackwards();
            }
            if (Input.GetKey(KeyCode.D))
            {
                GameCharacter.MoveRight();
            }


            UpdateCameraLocation();
            PlayerRotation();

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(pauseint % 2 == 0)
            {
                CloseMenu();
            }
            if(pauseint % 2 != 0)
            {
                CheckForInteraction();
            }
            
            pauseint += 1;
            
            
        }




    }
    void CloseMenu()
    {
        UIMasterObject.CloseStoreMenu();
        
        paused = false;
    }

    void UpdateCameraLocation()
    {
        Vector3 NewLocation = GameCharacter.transform.position + CameraOffset;
        Camera.transform.position = NewLocation;
    }
    void PlayerRotation()
    {
        GameCharacter.transform.Rotate(GameCharacter.transform.up, -Input.GetAxis("Mouse X") * mouseSensitivity, Space.World);
    }

    void CheckForInteraction()
    {
        for (int i = 0; i < SectionLocations.Length; i++)
        {
            if(Vector3.Distance(SectionLocations[i],GameCharacter.transform.position) < distanceThreshold)
            {
                RenderStoreMenu(SectionTypes[i]);
                paused = true;
            }
        }

        if(Vector3.Distance(CheckoutLocation, GameCharacter.transform.position) < distanceThreshold)
        {
            RenderPurchaseMenu();
            paused = true;
        }
    }

    void RenderStoreMenu(int itemType)
    {
        UIMasterObject.RenderStoreMenu(itemType);
    }

    void RenderPurchaseMenu()
    {
        UIMasterObject.RenderPurchaseMenu();
    }
}
