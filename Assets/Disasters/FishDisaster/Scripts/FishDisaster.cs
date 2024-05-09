using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FishDisaster : Disaster
{

    [SerializeField] private float spawnInterval = 0.5f; // how long between spawning each fish
    [SerializeField] private GameObject fish_prefab;

    private float timeUntilNextFish = 0.0f;

    //Constructor
    public FishDisaster()
    {
        lifeTime = 10f;
        boundaryBuffer = 2f;
    }


    void Start()
    {
        MultiplyScale();
        StartFishThrow();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ManageTimers();
        ManageFishThrow();
    }


    void ManageFishThrow()
    {
        timeUntilNextFish -= Time.deltaTime;
        if ( timeUntilNextFish <= 0 )
        {
            ThrowFish();
            timeUntilNextFish += spawnInterval;
        }
    }


    void StartFishThrow()
    {
        lifeTimer = lifeTime;
        //Choose a random direction to move (right (1) or left (-1))
        moveDirection = (Random.Range(0, 2) >= 1) ? (1) : (-1);
        //Set position
        transform.position = new Vector3((roomSize.x / 2) * -moveDirection, -(roomSize.y / 2) + boundaryBuffer, transform.position.z);
    }



    void ThrowFish()
    {
        //Create new fish
        GameObject newFish = Instantiate(fish_prefab, transform);
        SlappyFish newFishScript = newFish.GetComponent<SlappyFish>();
        //Set fish parent
        newFish.transform.parent = transform.parent;
        //Set fish variables
        newFishScript.direction = moveDirection;
        newFishScript.yBounds = -roomSize.y - 2;
    }

    public override void MultiplyScale()
    {
        base.MultiplyScale();
    }
}
