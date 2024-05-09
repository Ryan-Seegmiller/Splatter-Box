using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class CrowsDisaster : Disaster
{ 
    [SerializeField] private float minBombInterval = 3.0f; // the minimum amount of time between each bomb drop
    [SerializeField] private float maxBombInterval = 5.0f; // the maximum amount of time between each bomb drop

    [SerializeField] private float standardizedCrowHeight = 0.8f; // the height relative to the room size (-1 to 1) that the crows will fly
    [SerializeField] private GameObject crowBomb_prefab;
    [SerializeField] private float bombLifetime = 5.0f;

    private float timeUntilNextBomb = 0.0f;

    private Transform spriteContainer; //Cached sprite container for later use


    //Constructor
    public CrowsDisaster()
    {
        lifeTime = 8f;
        boundaryBuffer = 2f;
    }

    
    void Start()
    {
        MultiplyScale();
        //Cache sprite
        spriteContainer = transform.GetChild(0);

        StartMovement();

        timeUntilNextBomb = Random.Range( minBombInterval, maxBombInterval );
    }

    // Update is called once per frame
    void Update()
    {
        ManageTimers();
        ManageMovement();
        ManageBombDrop();
    }


    void ManageMovement()
    {
        //Get start and end positions
        float edge = ((roomSize.x / 2) + boundaryBuffer); //The right edge of the screen (or left edge if multiplied by -1)
        float startPos = edge * moveDirection;
        float endPos = edge * -moveDirection;
        //Lerp position over lifetime
        float xPosition = Mathf.Lerp(startPos, endPos, lifeTimer / lifeTime);
        transform.position = new Vector3(xPosition, (roomSize.y / 2) * standardizedCrowHeight, transform.position.z);
    }


    void ManageBombDrop()
    {
        timeUntilNextBomb -= Time.deltaTime;
        if ( timeUntilNextBomb <= 0 )
        {
            timeUntilNextBomb += Random.Range( minBombInterval, maxBombInterval );
            DropBomb();
        }

    }




    void StartMovement()
    {
        lifeTimer = lifeTime;
        //Choose a random direction to move (right (1) or left (-1))
        moveDirection = (Random.Range(0, 2) >= 1) ? (1) : (-1);
        //Flip sprites depending on move direction
        spriteContainer.localScale = new Vector2(moveDirection, 1);
    }


    void DropBomb()
    {
        //Create new bomb
        GameObject newBomb = Instantiate( crowBomb_prefab, transform.position, Quaternion.identity );
        //Set bomb parent
        newBomb.transform.parent = transform.parent;
        //Set bomb scale
        newBomb.transform.localScale = transform.localScale;

        newBomb.GetComponent< Rigidbody2D >().velocity = new Vector2( moveDirection * (roomSize.x + 2 * boundaryBuffer) / lifeTime, 0.0f );
    }

}
