using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float dropSpeed = 2f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashCooldown = 1.5f;
    [SerializeField] private Rigidbody2D rb, rb2, rb3, rb4;
    /*    [SerializeField] private InputActionAsset actionMap;*/
    [SerializeField] private Sprite[] sprites;  
 
    private Vector2 moveDirection;
    private Vector2 dashDirection;

    private float dashTimer = 0.0f;

    public bool canmove = false;
    public bool isStarted;
    public bool grounded;
    public bool sticky;
    private bool jump;
    private bool dashPress;
    private bool dashWasPress;
    private bool colorPressPlus;
    private bool colorWasPressPlus;
    private bool colorPressMinus;
    private bool colorWasPressMinus;
    private int spriteID = 0;

    //IM so sorry
    //I forgot to add array support
    private string[] jumpAudio;
    [SerializeField, Audio] string jumpAudio1;
    [SerializeField, Audio] string jumpAudio2;

    private string[] dashAudio;
    [SerializeField, Audio] string dashAudio1;
    [SerializeField, Audio] string dashAudio2;
    [SerializeField, Audio] string dashAudio3;

    [HideInInspector]public string[] collisionPlayerAudio;
    [SerializeField, Audio] string collisionPlayer1;
    [SerializeField, Audio] string collisionPlayer2;
    [SerializeField, Audio] string collisionPlayer3;
    [SerializeField, Audio] string collisionPlayer4;
    [SerializeField, Audio] string collisionPlayer5;
    [SerializeField, Audio] string collisionPlayer6;
    [SerializeField, Audio] string collisionPlayer7;
    [SerializeField, Audio] string collisionPlayer8;
    [SerializeField, Audio] string collisionPlayer9;

    [HideInInspector]public string[] collisionEnviormentAudio;
    [SerializeField, Audio] string collisionEnviorment1;
    [SerializeField, Audio] string collisionEnviorment2;
    [SerializeField, Audio] string collisionEnviorment3;

    private void Start()
    {
        jumpAudio = new string[] { jumpAudio1, jumpAudio2 };
        dashAudio = new string[] { dashAudio1, dashAudio2 };
        collisionPlayerAudio = new string[] {collisionPlayer1, collisionPlayer2, collisionPlayer3, collisionPlayer4, collisionPlayer5, collisionPlayer6, collisionPlayer7, collisionPlayer8, collisionPlayer9 };
        collisionEnviormentAudio = new string[] {collisionEnviorment1, collisionEnviorment2, collisionEnviorment3 };
    }


    private void Update()
    {
        GameStates currentState = GameManager.instance.currentState;
        bool inPlay = (currentState == GameStates.Play || currentState == GameStates.SuddenDeath);
        if (!canmove) return;

        // Ray2D ray = new Ray2D(transform.position, Vector2.down);
        if (!dashPress && inPlay)
        {
            //! You can move
            rb.AddForce(moveDirection * moveSpeed);
            rb2.AddForce(moveDirection * moveSpeed);
            rb3.AddForce(moveDirection * moveSpeed);
            rb4.AddForce(moveDirection * moveSpeed);

            //! If player collision force stop dash / disable
            if (dashWasPress && dashTimer <= 0.02)
            {
                rb.AddForce(dashDirection * dashSpeed);
                rb2.AddForce(dashDirection * dashSpeed);
                rb3.AddForce(dashDirection * dashSpeed);
                rb4.AddForce(dashDirection * dashSpeed);

                GameManager.instance.audioSourceHelper.PlayRandom(dashAudio, transform);

                dashTimer = dashCooldown;
            }
        }
        
        if (!colorPressPlus)
        {
            if (colorWasPressPlus)
            {
                print("yo");
                spriteID++;
                if (spriteID >= sprites.Length)
                    spriteID = 0;
                GetComponentInChildren<SpriteRenderer>().sprite = sprites[spriteID];
            }
        }
        if (!colorPressMinus)
        {
            if (colorWasPressMinus)
            {
                print("yo");
                spriteID--;
                if (spriteID < 0)
                    spriteID = sprites.Length - 1;
                GetComponentInChildren<SpriteRenderer>().sprite = sprites[spriteID];
            }
        }
        
        if (jump && grounded && inPlay)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            rb2.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            rb3.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            rb4.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

            GameManager.instance.audioSourceHelper.PlayRandom(jumpAudio, transform);

            jump = false;
            grounded = false;   
        }
        //For testing furposes only

        dashWasPress = dashPress;
        colorWasPressPlus = colorPressPlus;
        colorWasPressMinus = colorPressMinus;
        if (dashTimer > 0)
            dashTimer -= Time.deltaTime;
    }

    public void Teleport(Vector3 pos) {

        rb.position = pos + new Vector3(-.3f, .3f, 0.0f);
        rb2.position = pos + new Vector3(-.3f, -.3f, 0.0f);
        rb3.position = pos + new Vector3(.3f, .3f, 0.0f);
        rb4.position = pos + new Vector3(.3f, -.3f, 0.0f);

    }

    private void OnMove(InputValue value)
    {
        dashDirection = value.Get<Vector2>();
        moveDirection.x = value.Get<Vector2>().x;

    }
    private void OnJump(InputValue value)
    {
        float jumpValue = value.Get<float>();
        jump = jumpValue > 0;
    }

    private void OnStick(InputValue value)//InputAction.CallbackContext context)
    {
        sticky = value.Get<float>() > 0.5;

        print(value.Get<float>());
        print(sticky);
    }

    private void OnDash(InputValue value)//InputAction.CallbackContext context)
    {
        
        dashPress = value.Get<float>() > 0.5;

        print(value.Get<float>());
        print(dashPress);
    }

    private void OnColorSelect(InputValue value)
    {
        colorPressPlus = value.Get<Vector2>().y > 0.5;
        colorPressMinus = value.Get<Vector2>().y < -0.5;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down);

    }
}
