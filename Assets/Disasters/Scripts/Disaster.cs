using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disaster : MonoBehaviour
{
    public float lifeTime;

    public Vector2 roomSize = new Vector2(40f, 20f); //Total size of the room
    [HideInInspector] public float lifeTimer;
    [HideInInspector] public float boundaryBuffer; //The extra buffer area on the ends of the room
    [HideInInspector] public float moveDirection; //Crow's move direction. 1 = right, -1 = left, 0 = no movement.
    [HideInInspector] public float scale = 1f; //The scale of the window based on the default 20x10 unit camera size



    void Start()
    {

    }


    void Update()
    {

    }


    public void ManageTimers()
    {
        if (lifeTimer > 0)
        {
            lifeTimer -= Time.deltaTime;
            //Timeout
            if (lifeTimer <= 0)
            {
                lifeTimer = 0f;
                Remove();
            }
        }
    }


    //Remove the disaster
    void Remove()
    {
        Destroy(gameObject);
    }


    //Multiplies position values based on scale
    public virtual void MultiplyScale()
    {

        //Scale variables
        scale = roomSize.x / 20;
        transform.localScale = new Vector3(scale, scale, scale);
        boundaryBuffer *= scale;
    }



}
