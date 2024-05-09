using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrainDisaster : Disaster
{
    [SerializeField] Transform spriteContainer;

    float yPositionBuffer = 8f; //The y position buffer area from the top and bottom of the screen

    float trainSize = 31f; //HALF the train's size on the x axis (plus 1 extra unit)


    //Constructor
    public TrainDisaster()
    {
        lifeTime = 4f;
        boundaryBuffer = 2f;
    }




    void Start()
    {
        MultiplyScale();

        //Cache sprite
        spriteContainer = transform.GetChild(0);

        StartMovement();
    }
    

    void Update()
    {
        ManageTimers();
        ManageMovement();
    }


    void ManageMovement()
    {
        //Get start and end positions
        float edge = ((roomSize.x / 2) + trainSize); //The right edge of the screen (or left edge if multiplied by -1)
        float startPos = edge * -moveDirection;
        float endPos = edge * moveDirection;
        //Lerp position over lifetime
        float xPosition = Mathf.Lerp(startPos, endPos, lifeTimer / lifeTime);
        transform.position = new Vector3(xPosition, transform.position.y, transform.position.x);
    }


    void StartMovement()
    {
        lifeTimer = lifeTime;
        //Choose a random direction to move (right (1) or left (-1))
        moveDirection = (Random.Range(0, 2) >= 1) ? (1) : (-1);
        //Set y position
        float yEdge = (roomSize.y / 2);
        transform.position = new Vector3(transform.position.x, -yEdge + Random.Range(yPositionBuffer, yEdge / 2 + (yEdge / 6)), transform.position.z);
        //Flip sprites depending on move direction
        //spriteContainer.localScale = new Vector2(moveDirection, 1);
    }


    public override void MultiplyScale()
    {
        base.MultiplyScale();
        yPositionBuffer *= scale;
        trainSize *= scale;
    }

}
