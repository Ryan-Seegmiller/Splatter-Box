using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingAnimal : MonoBehaviour
{

    Vector2 fallSpeed = new Vector2(4.0f, 12.0f);
    Vector2 speedVariation = new Vector2(0.6f, 0.2f);
    public float yBounds = -11f; //The y boundaries. (Destroy object if it goes below this point)
    public float direction = 1f; //-1 = left, 1 = right

    void Start()
    {
        StartMovement();
        MultiplyScale();
    }


    void Update()
    {
        ManageMovement();
        ManageBounds();
    }


    void ManageMovement()
    {
        transform.position += new Vector3(fallSpeed.x * direction, -fallSpeed.y, 0) * Time.deltaTime;
    }


    void ManageBounds()
    {
        if (transform.position.y < yBounds)
        {
            Destroy(gameObject);
        }
    }



    void StartMovement()
    {
        //Set fall speed
        fallSpeed.x *= Random.Range(1 - speedVariation.x, 1 + speedVariation.x);
        fallSpeed.y *= Random.Range(1 - speedVariation.y, 1 + speedVariation.y);
        //Set direction
        direction = (Random.Range(0, 2) >= 1) ? (1) : (-1);

        //Set animal
        int animal = (Random.Range(0, 2) >= 1) ? (1) : (0);
        transform.GetChild(animal).gameObject.SetActive(true);
        //Set rotation
        transform.GetChild(animal).Rotate(new Vector3(0, 0, Random.Range(0, 359)));
    }


    void MultiplyScale()
    {
        fallSpeed *= transform.localScale.x;
        yBounds *= transform.localScale.x;
    }
}
