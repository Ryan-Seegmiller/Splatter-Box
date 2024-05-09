using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Squish : MonoBehaviour
{
    public Transform spriteTR;
    public float Stretch = 0.1f;

    private Rigidbody2D rb;
    private Vector3 originalScale;

    

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = spriteTR.transform.localScale;
    }

    private void FixedUpdate()
    {
        var velocity = rb.velocity;

        var scaleX = originalScale.x + (velocity.magnitude * Stretch);
        var scaleY = originalScale.y / scaleX;
        transform.localScale = new Vector3(scaleX, scaleY, 1.0f);
    }
    
}
