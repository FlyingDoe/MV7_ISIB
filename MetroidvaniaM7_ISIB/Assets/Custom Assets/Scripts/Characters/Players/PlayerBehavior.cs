using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : CharacterBehavior
{
    [SerializeField] Rigidbody PlayerRB;
    [SerializeField] int numOfHearts;
    [SerializeField] new float JumpPower;
    [SerializeField] new float MoveSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] Collider biteCollider;
    [SerializeField] Collider headCollider;

    public Image[] Hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public string state;
    public float fallMultiplier = 2.5f;
    public float speedJumpMultiplier = 20.0f;
    public float dashTime;
    public float timecooldownDash;
    public float maxDurationBite;

    private float durationBite;
    private float startDashTime;
    private int dashDirection;
    private float horizontalMovement;
    private bool isGrounded;
 

    // Start is called before the first frame update
    void Start()
    {
        MaxHp = numOfHearts;
        Hp = numOfHearts -1;
        state = "IDLE";
        dashDirection = 6;
        timecooldownDash = 4;
    }

    private void FixedUpdate()
    {
        timecooldownDash += Time.deltaTime;
        isIdle(Physics.Raycast(transform.position, Vector3.down, 0.2f));        
        if (PlayerRB.velocity.y < 0)
        {
            PlayerRB.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (PlayerRB.velocity.y > 0)
        {
            transform.Translate(0, speedJumpMultiplier * Time.deltaTime, 0);
        }

        switch (state)
        {
            case "IDLE":
                if (Input.GetButtonDown("Jump")) //Project settings input
                {
                    PlayerRB.AddForce(Vector3.up * JumpPower);
                    state = "JUMP";
                }
                if (Input.GetButton("Horizontal")) //Project settings input
                {
                    if (Input.GetAxisRaw("Horizontal") < 0)
                    {
                        transform.eulerAngles = new Vector3(0, 180, 0);
                        transform.Translate(MoveSpeed * Time.deltaTime, 0, 0);
                        dashDirection = 5;
                    } else if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        transform.Translate(MoveSpeed * Time.deltaTime, 0, 0); // A CHANGER SI ON NE VEUT PAS SE STOP A H = 0
                        dashDirection = 6;
                    }
                    Debug.Log("H" + Input.GetAxisRaw("Horizontal"));
                }

                if (Input.GetButtonDown("Dash") && timecooldownDash >= 4) //Project settings input
                {
                    dashDirection = getDashDirection(); //A TERMINER !!! dashtime -= Time.deltaTime et si plus petit que 0 = finito le state ne sera plus en dash
                    state = "DASH";
                    Debug.Log("DASH Input Idle");
                    startDashTime = 0;
                }

                if (Input.GetButtonDown("AttackMelee"))
                {
                    biteCollider.enabled = true;
                    headCollider.enabled = false;
                    durationBite = maxDurationBite;
                    state = "ATK_MELEE";
                }
                break;

            case "JUMP":
                if (Input.GetButton("Horizontal")) //Project settings input
                {
                    if (Input.GetAxisRaw("Horizontal") < 0)
                    {
                        transform.eulerAngles = new Vector3(0, 180, 0);
                        transform.Translate(MoveSpeed * Time.deltaTime, 0, 0);
                        dashDirection = 5;
                    }
                    else if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        transform.Translate(MoveSpeed * Time.deltaTime, 0, 0);
                        dashDirection = 6;
                    }
                    Debug.Log("H" + Input.GetAxisRaw("Horizontal"));
                }

                if (Input.GetButtonDown("Dash") && timecooldownDash >= 4) //Project settings input
                {
                    dashDirection = getDashDirection(); //A TERMINER !!! dashtime -= Time.deltaTime et si plus petit que 0 = finito le state ne sera plus en dash
                    state = "DASH";
                    Debug.Log("Start DASH Input Jump");
                    startDashTime = 0;
                }

                if (Input.GetButtonDown("AttackMelee"))
                {
                    biteCollider.enabled = true;
                    headCollider.enabled = false;
                    durationBite = maxDurationBite;
                    state = "ATK_MELEE";
                }

                break;

            case "DASH":
                if (startDashTime < dashTime)
                {
                    startDashTime += Time.deltaTime;
                    dash(dashDirection);
                    Debug.Log("DASH");
                } else { 
                endDash(dashDirection);
                isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.2f);//VOIR LA BONNE DISTANCE
                if (isGrounded)                                      
                {
                    state = "IDLE";
                } else
                {
                    state = "JUMP";
                }
                timecooldownDash = 0;
                Debug.Log("Stop Dash");
                }

                break;

            case "ATK_MELEE":
                durationBite -= Time.deltaTime;
                if (durationBite <= 0)
                {
                    biteCollider.enabled = false;
                    headCollider.enabled = true;                
                isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.2f);//VOIR LA BONNE DISTANCE
                if (isGrounded)
                {
                    state = "IDLE";
                }
                else
                {
                    state = "JUMP";
                }
                Debug.Log("Stop AttackBite");
                }


                break;

            case "ATK_DISTANCE":

                break;
        }

    }

    private void isIdle(bool isGrounded)
    {
        if (isGrounded && state != "DASH" && state != "ATK_MELEE")
        {
            state = "IDLE";
        }
         
    }

    // Update calcule du nombre de coeur
    void Update()
    {

        if (Hp > numOfHearts)
        {
            Hp = numOfHearts;
        }
        for (int i = 0; i < Hearts.Length; i++)
        {
            if (i < Hp)
            {
                Hearts[i].sprite = fullHeart;
            } else
            {
                Hearts[i].sprite = emptyHeart;
            }
            if (i < numOfHearts)
            {
                Hearts[i].enabled = true;
            } else
            {
                Hearts[i].enabled = false;
            }

        }
    }
 

    private int getDashDirection() //8 direction possible
    {
        if (Input.GetButton("Horizontal") && Input.GetButton("Vertical")) //Project settings input
        {
            if (Input.GetAxisRaw("Horizontal") < 0 && Input.GetAxisRaw("Vertical") < 0) // En bas à gauche
            {
                return 1;
            }
            if (Input.GetAxisRaw("Horizontal") > 0 && Input.GetAxisRaw("Vertical") < 0) // En bas à droite
            {
                return 2;
            }
            if (Input.GetAxisRaw("Horizontal") < 0 && Input.GetAxisRaw("Vertical") > 0) // En haut à gauche
            {
                return 3;
            }
            if (Input.GetAxisRaw("Horizontal") > 0 && Input.GetAxisRaw("Vertical") > 0) // En haut à droite
            {
                return 4;
            }

        }

        if (Input.GetButton("Horizontal")) //Project settings input
        {
            if (Input.GetAxisRaw("Horizontal") < 0) // A gauche
            {
                return 5;
            } else                                  // A droite
            {
                return 6;
            }
        }

        if (Input.GetButton("Vertical")) //Project settings input
        {
            if (Input.GetAxisRaw("Vertical") < 0) // En bas
            {
                return 7;
            }
            else                                  // En haut
            {
                return 8;
            }
        }
        return dashDirection;
    }
    private void dash(int direction)
    {
        switch (direction)
        {
            case 1:
                PlayerRB.velocity = Vector3.left * dashSpeed / 2 + Vector3.down * dashSpeed / 2;                
                break;

            case 2:
                PlayerRB.velocity += Vector3.right * dashSpeed / 2 + Vector3.down * dashSpeed / 2;
                break;

            case 3:
                PlayerRB.velocity += Vector3.left * dashSpeed / 2 + Vector3.up * dashSpeed / 2;
                break;

            case 4:
                PlayerRB.velocity += Vector3.right * dashSpeed / 2 + Vector3.up * dashSpeed / 2;
                break;

            case 5:
                PlayerRB.velocity += Vector3.left * dashSpeed;
                break;

            case 6:               
                PlayerRB.velocity += Vector3.right * dashSpeed;
                break;

            case 7:
                PlayerRB.velocity += Vector3.down * dashSpeed;
                break;

            case 8:
                PlayerRB.velocity += Vector3.up * dashSpeed;
                break;

        }
    }
    private void endDash(int direction)
    {
        switch (direction)
        {
            case 1:
                PlayerRB.velocity = Vector3.left * MoveSpeed/2 + Vector3.down * MoveSpeed / 2;
                break;

            case 2:
                PlayerRB.velocity = Vector3.right * MoveSpeed / 2 + Vector3.down * MoveSpeed / 2;
                break;

            case 3:
                PlayerRB.velocity = Vector3.left * MoveSpeed / 2 + Vector3.up * MoveSpeed / 2;
                break;

            case 4:
                PlayerRB.velocity = Vector3.right * MoveSpeed / 2 + Vector3.up * MoveSpeed / 2;
                break;

            case 5:
                PlayerRB.velocity = Vector3.left * MoveSpeed;
                break;

            case 6:
                PlayerRB.velocity = Vector3.right * MoveSpeed;
                break;

            case 7:
                PlayerRB.velocity = Vector3.down * MoveSpeed;
                break;

            case 8:
                PlayerRB.velocity = Vector3.up * MoveSpeed;
                break;

        }
    }
    

}
