using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoDisaster : Disaster
{
    [SerializeField] private float dropTime = .4f; // How much of the lifetime is used for dropping the piano. (Affects drop speed)
    [SerializeField] private float timeVariation = 0.5f; // The random amount that the time varies
    [SerializeField] private float horizontalSpeed = 6f; // The speed that the piano moves back and forth before dropping

    [SerializeField] private Vector2 startPositionBuffer = new Vector2(2.2f, 1.2f); //The distance from the edge and top of the screen that the piano starts at

    private float startYPos;
    private bool hasDropped = false;

    //Constructor
    public PianoDisaster()
    {
        lifeTime = 7f;
        boundaryBuffer = 3f;
    }




    void Start()
    {
        MultiplyScale();

        StartMovement();
    }



    void Update()
    {
        ManageTimers();
        ManageMovement();
    }



    void ManageMovement()
    {
        //Back and forth movement
        if (lifeTimer > dropTime)
        {
            transform.position = new Vector3(transform.position.x + horizontalSpeed * moveDirection * Time.deltaTime, startYPos, 0);

            //Reverse direction
            if (transform.position.x > (roomSize.x / 2) - boundaryBuffer)
            {
                moveDirection = -1;
            }
            else if (transform.position.x < -(roomSize.x / 2) + boundaryBuffer)
            {
                moveDirection = 1;
            }
        }
        //Drop movement
        else if ( hasDropped == false )
        {
            hasDropped = true;
            GetComponent< Rigidbody2D >().isKinematic = false;
        }
    }


    void StartMovement()
    {
        //Randomize lifetime
        dropTime += Random.Range( -timeVariation, timeVariation );
        //Choose a random direction to move (right (1) or left (-1))
        moveDirection = (Random.Range(0, 2) >= 1) ? (1) : (-1);
        //Set start position
        startYPos = (roomSize.y / 2) - startPositionBuffer.y;
        float edge = (roomSize.x / 2) + startPositionBuffer.x;
        transform.position = new Vector3(edge * moveDirection, startYPos, transform.position.z);

        GetComponent< Rigidbody2D >().isKinematic = true;
    }

    public override void MultiplyScale()
    {
        base.MultiplyScale();
        startPositionBuffer *= scale;
        horizontalSpeed *= scale;
    }

}
