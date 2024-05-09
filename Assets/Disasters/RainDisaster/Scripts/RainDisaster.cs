using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainDisaster : Disaster
{
    [SerializeField] int animalCount = 30; //How many animals to drop over lifetime
    [SerializeField] GameObject animal_prefab;

    int animalsDropped = 0; //The number of animals that have been dropped


    //Constructor
    public RainDisaster()
    {
        lifeTime = 10f;
        boundaryBuffer = 4f;
    }




    void Start()
    {
        MultiplyScale();

        StartRain();
    }

    // Update is called once per frame
    void Update()
    {
        ManageTimers();
        ManageRain();
    }


    void ManageRain()
    {
        //Throw fish if the time is right
        float throwTime = (lifeTime / animalCount) * (animalCount - animalsDropped);
        if (lifeTimer < throwTime)
        {
            DropAnimal();
        }
    }


    void StartRain()
    {
        lifeTimer = lifeTime;
        //Set position
        transform.position = new Vector3(0, (roomSize.y / 2) - 2f, transform.position.z);
    }



    void DropAnimal()
    {
        animalsDropped++;
        //Create new animal
        GameObject newAnimal = Instantiate(animal_prefab, transform);
        FallingAnimal newAnimalScript = newAnimal.GetComponent<FallingAnimal>();
        //Set animal parent
        newAnimal.transform.parent = transform.parent;
        //Set animal position
        float xPos = Random.Range(-(roomSize.x / 2) + boundaryBuffer, (roomSize.x / 2) - boundaryBuffer);
        newAnimal.transform.position = new Vector3(xPos, roomSize.y / 2, transform.position.z);
        //Set animal scale
    }

    public override void MultiplyScale()
    {
        base.MultiplyScale();
    }
}