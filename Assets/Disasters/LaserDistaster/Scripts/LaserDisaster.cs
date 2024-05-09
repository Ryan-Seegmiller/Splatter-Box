using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LaserDisaster : Disaster
{

    [SerializeField] float speed = 3f; //Back and forth movement speed
    [SerializeField] float speedVariation = 0.5f; //The random variation in speed
    [SerializeField] float spawnDelay = 1.0f;

    float yPositionBuffer = 0.5f; //The y position buffer area from the top and bottom of the screen

    Vector2 laserDirection = new Vector2(1, -1); //The move directions for each laser (x = bottom, y = top)
    Vector2 laserSpeed; //The speed for each laser (x = bottom, y = top)

    Transform bottomLaser;
    Transform topLaser;

    RaycastHit2D bottomHit;
    RaycastHit2D topHit;

    public LaserDisaster()
    {
        lifeTime = 10f;
        boundaryBuffer = 1f;
    }


    void Start()
    {
        MultiplyScale();

        //Cache lasers
        //bottomLaser = transform.GetChild(0);
        topLaser = transform.GetChild(1);

        StartMovement();
    }


    void Update()
    {
        ManageTimers();
        ManageMovement();
        if ( lifeTime - lifeTimer > spawnDelay )
        {
            ManageRaycast();
            DrawLaser();
        }
    }



    void ManageMovement()
    {
        //Move
        //bottomLaser.position += new Vector3(laserSpeed.x * laserDirection.x, 0, 0) * Time.deltaTime;
        topLaser.position += new Vector3(laserSpeed.y * laserDirection.y, 0, 0) * Time.deltaTime;

        /*
        //CHANGE DIRECTIONS
        //Bottom laser right side
        if (bottomLaser.position.x >= (roomSize.x / 2) - boundaryBuffer)
        { laserDirection.x = -1; }
        //Bottom laser left side
        else if (bottomLaser.position.x <= -(roomSize.x / 2) + boundaryBuffer)
        { laserDirection.x = 1; }
        */

        //Top laser right side
        if (topLaser.position.x >= (roomSize.x / 2) - boundaryBuffer)
        { laserDirection.y = -1; }
        //Top laser left side
        else if (topLaser.position.x <= -(roomSize.x / 2) + boundaryBuffer)
        { laserDirection.y = 1; }
    }

    void ManageRaycast()
    {
        //Bottom laser
        //bottomHit = Physics2D.Raycast(bottomLaser.position, Vector2.up, roomSize.y);
        //Top laser
        topHit = Physics2D.Raycast(topLaser.position, -Vector2.up, roomSize.y);

        //print(topHit.collider.gameObject.name);

        //if (bottomHit)
        //{ bottomHit.collider.gameObject.GetComponent<PlayerState>().KillPlayer(); }
        if (topHit)
        {
            if (topHit.collider.gameObject.transform.parent.parent.TryGetComponent(out PlayerState player))
            {
                player.KillPlayer();
            }
        }
    }

    void DrawLaser()
    {
        //Get lines
        //LineRenderer bottomLine = bottomLaser.GetComponent<LineRenderer>();
        //Get length
        //Vector3 bottomEndPoint = (bottomHit == true) ? (bottomHit.point) : (bottomLaser.transform.position + new Vector3(0, roomSize.y, 0));
        //Bottom laser
        //bottomLine.SetPosition(0, bottomLaser.transform.position);
        //bottomLine.SetPosition(1, bottomEndPoint);

        LineRenderer topLine = topLaser.GetComponent<LineRenderer>();
        Vector3 topEndPoint = (topHit == true) ? (topHit.point) : (topLaser.transform.position - new Vector3(0, roomSize.y, 0));
        topLine.SetPosition(0, topLaser.transform.position);
        topLine.SetPosition(1, topEndPoint);

    }


    void StartMovement()
    {
        //Set laser directions (Lasers will start off going opposite directions
        float directionChoice = (Random.Range(0, 2) >= 1) ? (1) : (-1);
        //laserDirection.x = directionChoice;
        laserDirection.y = -directionChoice;

        //Set start position
        float edge = ((roomSize.x / 2) - boundaryBuffer);
        //bottomLaser.position = new Vector3(edge * -laserDirection.x, -(roomSize.y / 2) + yPositionBuffer, transform.position.z);
        topLaser.position = new Vector3(edge * -laserDirection.y, (roomSize.y / 2) - yPositionBuffer, transform.position.z);

        //Set laser speed
        //laserSpeed.x = speed * Random.Range(1 - speedVariation, 1 + speedVariation);
        laserSpeed.y = speed * Random.Range(1 - speedVariation, 1 + speedVariation);
        //Scale speed
        laserSpeed *= scale;
    }


    public override void MultiplyScale()
    {
        base.MultiplyScale();
        yPositionBuffer *= scale;
    }






    /*
    private void OnDrawGizmos()
    {
        if (bottomLaser != null && topLaser != null)
        {
            float bottomLength = (bottomHit == true) ? (Mathf.Abs(bottomHit.point.y - bottomLaser.position.y)) : (roomSize.y);
            float topLength = (topHit == true) ? (Mathf.Abs(topHit.point.y - topLaser.position.y)) : (roomSize.y);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(bottomLaser.position, bottomLaser.position + new Vector3(0, bottomLength, 0));
            Gizmos.DrawLine(topLaser.position, topLaser.position - new Vector3(0, topLength, 0));

        }
    }*/
}
