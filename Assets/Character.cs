using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public float CharacterMovementSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveForward()
    {
        rb.velocity = transform.forward * CharacterMovementSpeed;
    }

    public void MoveBackwards()
    {
        rb.velocity = transform.forward * -CharacterMovementSpeed;
    }

    public void MoveLeft()
    {
        rb.velocity = transform.right * -CharacterMovementSpeed;
    }

    public void MoveRight()
    {
        rb.velocity = transform.right * CharacterMovementSpeed;
    }
}
