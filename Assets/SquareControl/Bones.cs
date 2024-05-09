using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Bones : MonoBehaviour
{
    [SerializeField] private PlayerMovement controller;
    private Rigidbody2D rb;

    bool wasfrozen = false;
    [SerializeField] private PlayerState playerstate;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wasfrozen && controller.sticky == false)
        {
            rb.constraints &= ~RigidbodyConstraints2D.FreezeAll;
        }
        wasfrozen = controller.sticky;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Wall" || 
            (collision.gameObject.tag == "Player" && 
            collision.transform.parent.parent.gameObject != transform.parent.parent.gameObject))
        {
            if(collision.gameObject.tag == "Player") { 
                GameManager.instance.audioSourceHelper.PlayRandom(controller.collisionEnviormentAudio, transform);
            }
            else
            {
                GameManager.instance.audioSourceHelper.PlayRandom(controller.collisionPlayerAudio, transform);
            }
            controller.grounded = true;
            
        }
        if ((collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Ground") && controller.sticky == true)
        {

            rb.constraints |= RigidbodyConstraints2D.FreezeAll;
        }
        if (collision.gameObject.tag == "Deadly") {
            playerstate.KillPlayer();
        }
    }
}
