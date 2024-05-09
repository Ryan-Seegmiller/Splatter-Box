using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlappyFish : MonoBehaviour
{
    [SerializeField] Vector2 launchRange = new Vector2(8, 24); //The min and max launch strengths
    float maxTorque = 14; //The maximum amount of torque that can be given when launched

    public float yBounds = -11f; //The y boundaries. (Destroy object if it goes below this point)

    public float direction = 1f; //-1 = left, 1 = right

    Rigidbody2D rb;

    void Start()
    {
        //Get components
        rb = GetComponent<Rigidbody2D>();

        MultiplyScale();
        Launch();
    }


    void FixedUpdate()
    {
        ManageBounds();
    }



    void ManageBounds()
    {
        if (transform.position.y < yBounds)
        {
            Destroy(gameObject);
        }
    }


    void Launch()
    {
        //Add force
        Vector2 newForce = new Vector2(
            Random.Range(launchRange.x, launchRange.y),
            Random.Range(launchRange.x, launchRange.y)) * rb.mass;
        rb.AddForce(new Vector3(newForce.x * direction, newForce.y, 0), ForceMode2D.Impulse);
        //Add torque
        rb.AddTorque(Random.Range(-maxTorque, maxTorque));
    }

    void MultiplyScale()
    {
        yBounds *= transform.localScale.x;
        maxTorque *= transform.localScale.y;
        //Scale launch
        float launchMultiplier = 1 - ((1 - (transform.localScale.x)) / 2);
        launchRange *= launchMultiplier;
    }

}
