using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandDisaster : Disaster
{

    [SerializeField] private float yPositionBuffer = 4f; //The y position buffer area from the top and bottom of the screen
    [SerializeField] private float handSpeed = 1.0f;
    

    //Constructor
    public HandDisaster()
    {
        lifeTime = 4f;
        boundaryBuffer = 2f;
    }




    void Start()
    {
        MultiplyScale();

        StartMovement();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ManageTimers();
        ManageMovement();
    }


    void ManageMovement()
    {
        transform.position += new Vector3( handSpeed * moveDirection * Time.deltaTime, 0.0f, 0.0f );
    }


    void StartMovement()
    {
        lifeTimer = lifeTime;
        //Choose a random direction to move (right (1) or left (-1))
        moveDirection = (Random.Range(0, 2) >= 1) ? (1) : (-1);
        //Set y position
        float yEdge = (roomSize.y / 2) - yPositionBuffer;
        transform.position = new Vector3((roomSize.x / 2 + boundaryBuffer) * -moveDirection, Random.Range(-yEdge, yEdge), transform.position.z);
        //Flip sprites depending on move direction
        transform.localScale = new Vector2(moveDirection, 1);
    }


    public override void MultiplyScale()
    {
        base.MultiplyScale();
        yPositionBuffer *= scale;
    }
}
