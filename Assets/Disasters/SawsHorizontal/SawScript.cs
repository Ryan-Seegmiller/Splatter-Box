using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawScript : MonoBehaviour
{
    [SerializeField] private GameObject[] leftSaws;
    [SerializeField] private GameObject[] rightSaws;

    [SerializeField] private float moveToMid = 15.0f;
    private float totalMove = 0.0f;
    bool towards = true;
    private float totalMoveR = 0.0f;
    bool towardsR = true;

    // Start is called before the first frame update
    void Start()
    {
        totalMove = moveToMid;
        totalMoveR = moveToMid;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject left in leftSaws)
        {
            if (totalMove > 0.01f && towards)
            {
                left.transform.position += new Vector3(1f * Time.deltaTime, 0, 0);
                totalMove -= Time.deltaTime;
                
                if (totalMove <= 0.01f)
                {
                    towards = !towards;
                    totalMove = moveToMid;
                }
            }
            if (totalMove > 0.01f && !towards)
            {
                left.transform.position -= new Vector3(1f * Time.deltaTime, 0, 0);
                totalMove -= Time.deltaTime;

                if (totalMove <= 0.01f)
                {
                    towards = !towards;
                    totalMove = moveToMid;
                }
            }
            

            left.transform.Rotate(new Vector3(0, 0, 5));
        }
        foreach (GameObject right in rightSaws)
        {
            if (totalMoveR > 0.01f && towardsR)
            {
                right.transform.position -= new Vector3(1f * Time.deltaTime, 0, 0);
                totalMoveR -= Time.deltaTime;

                if (totalMoveR <= 0.01f)
                {
                    towardsR = !towardsR;
                    totalMoveR = moveToMid;
                }
            }
            if (totalMoveR > 0.01f && !towardsR)
            {
                right.transform.position += new Vector3(1f * Time.deltaTime, 0, 0);
                totalMoveR -= Time.deltaTime;

                if (totalMoveR <= 0.01f)
                {
                    towardsR = !towardsR;
                    totalMoveR = moveToMid;
                }
            }


            right.transform.Rotate(new Vector3(0, 0, 5));
        }
    }

}
