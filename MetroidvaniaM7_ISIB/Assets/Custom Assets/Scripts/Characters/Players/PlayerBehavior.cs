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
    [SerializeField] Collider[] tailCollider;

    public Image[] Hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public string state;
    public float fallMultiplier = 2.5f;
    public float speedJumpMultiplier = 20.0f;
    public float dashTime;
    public float timecooldownDash;
    public float maxDurationBite;
    public float maxDurationTail;
    public float maxDurationSmell;
    public int numFireBall;

    private Animator animator;

    private float durationSmell;
    private float durationBite;
    private float durationTail;
    private float startDashTime;
    private int dashDirection;
    private float horizontalMovement;

    private Vector3 previousPosition;
    private Vector3 currentPosition;

    ObjectPooler objectPooler;

    bool IsFalling
    {
        get { return currentPosition.y < previousPosition.y; }
    }

    bool IsGrounded
    {
        get { return Physics.Raycast(transform.position, Vector3.down, 0.2f); }//VOIR LA BONNE DISTANCE
    }


    // Start is called before the first frame update
    void Start()
    {
        MaxHp = numOfHearts;
        Hp = numOfHearts -1;
        state = "IDLE";
        dashDirection = 6;
        timecooldownDash = 4;
        objectPooler = ObjectPooler.Instance;
        currentPosition = transform.position;
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        // we may want to check in which direction it's moving
        // if any
        previousPosition = currentPosition;
        currentPosition = transform.position;

        timecooldownDash += Time.deltaTime;
        isIdle();
        if (PlayerRB.velocity.y < 0)
        {
            PlayerRB.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (PlayerRB.velocity.y > 0)
        {
            transform.Translate(0, speedJumpMultiplier * Time.deltaTime, 0);
        }

        #region StateManager

        switch (state)
        {
            case "IDLE":
                if (Input.GetButtonDown("Jump")) //Project settings input
                {
                    PlayerRB.AddForce(Vector3.up * JumpPower);
                    animator.SetTrigger("jump");
                    state = "JUMP";
                }
                if (Input.GetButton("Horizontal")) //Project settings input
                {
                    if (Input.GetAxisRaw("Horizontal") < 0)
                    {
                        transform.eulerAngles = new Vector3(0, 180, 0);
                        transform.Translate(MoveSpeed * Time.deltaTime, 0, 0);
                        dashDirection = 5;
                        animator.SetBool("running", true);
                    } else if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        transform.Translate(MoveSpeed * Time.deltaTime, 0, 0); // A CHANGER SI ON NE VEUT PAS SE STOP A H = 0
                        dashDirection = 6;
                        animator.SetBool("running", true);
                    } 
                    Debug.Log("H" + Input.GetAxisRaw("Horizontal"));
                } else
                {                
                        animator.SetBool("running", false);                    
                }

                if (Input.GetButtonDown("Dash") && timecooldownDash >= 4) //Project settings input
                {
                    dashDirection = getDashDirection(); //A TERMINER !!! dashtime -= Time.deltaTime et si plus petit que 0 = finito le state ne sera plus en dash
                    state = "DASH";
                    startDashTime = 0;
                }

                if (Input.GetButtonDown("AttackMelee"))
                {
                    biteCollider.enabled = true;
                    headCollider.enabled = false;
                    durationBite = maxDurationBite;
                    animator.SetBool("atk1", true);
                    state = "ATK_MELEE_BITE";
                }

                if (Input.GetKeyDown(KeyCode.R)) //ATK Tail A CHANGER LES COLLIDER 
                {
                    foreach (var collider in tailCollider)
                    {
                        collider.enabled = true;
                    }
                    durationTail = maxDurationTail;
                    animator.SetBool("atk2", true);
                    state = "ATK_MELEE_TAIL";
                }

                if (Input.GetKeyDown(KeyCode.F)) //ATK Tail A CHANGER LES COLLIDER 
                {
                    durationSmell = maxDurationSmell;
                    animator.SetBool("smelll", true);
                    state = "SMELL";
                }

                if (Input.GetKeyDown(KeyCode.G)) //ATK Tail A CHANGER LES COLLIDER 
                {
                    if (numFireBall > 0)
                    {
                        numFireBall -= 1;
                        animator.SetBool("atk1", true);
                        Debug.Log(transform.rotation.y);
                        bool directionRightFireBall = true;
                        if (transform.rotation.y == 1)
                        {
                            directionRightFireBall = false;
                        }
                        objectPooler.SpawnFromPool("FireBall", transform.position, Quaternion.identity, directionRightFireBall);
                        state = "ATK_DISTANCE_FIREBALL";
                    }
                }
                break;

            case "JUMP":
                if (Input.GetButton("Horizontal")) //Project settings input
                {
                    if (Input.GetAxisRaw("Horizontal") < 0)
                    {
                        animator.SetBool("running", true);
                        transform.eulerAngles = new Vector3(0, 180, 0);
                        transform.Translate(MoveSpeed * Time.deltaTime, 0, 0);
                        dashDirection = 5;
                    }
                    else if (Input.GetAxisRaw("Horizontal") > 0)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        transform.Translate(MoveSpeed * Time.deltaTime, 0, 0);
                        animator.SetBool("running", true);
                        dashDirection = 6;
                    }
                    Debug.Log("H" + Input.GetAxisRaw("Horizontal"));
                }

                if (Input.GetButtonDown("Dash") && timecooldownDash >= 4) //Project settings input
                {
                    dashDirection = getDashDirection(); //A TERMINER !!! dashtime -= Time.deltaTime et si plus petit que 0 = finito le state ne sera plus en dash
                    state = "DASH";
                    startDashTime = 0;
                }

                if (Input.GetButtonDown("AttackMelee"))
                {
                    biteCollider.enabled = true;
                    headCollider.enabled = false;
                    durationBite = maxDurationBite;
                    animator.SetBool("atk1", true);
                    state = "ATK_MELEE_BITE";
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    foreach (var collider in tailCollider)
                    {
                        collider.enabled = true;
                    }
                    durationTail = maxDurationTail;
                    animator.SetBool("atk2", true);
                    state = "ATK_MELEE_TAIL";
                }
                if (Input.GetKeyDown(KeyCode.G)) 
                {
                    if (numFireBall > 0)
                    {
                        numFireBall -= 1;
                        animator.SetBool("atk1", true);
                        Debug.Log(transform.rotation.y);
                        bool directionRightFireBall = true;
                        if (transform.rotation.y == 1)
                        {
                            directionRightFireBall = false;
                        }
                        objectPooler.SpawnFromPool("FireBall", transform.position, Quaternion.identity, directionRightFireBall);
                        state = "ATK_DISTANCE_FIREBALL";
                    }
                }

                break;



            case "DASH":
                if (startDashTime < dashTime)
                {
                    startDashTime += Time.deltaTime;
                    dash(dashDirection);
                } else { 
                endDash(dashDirection);
                if (IsGrounded)
                {
                    state = "IDLE";
                } else
                {
                    state = "JUMP";
                }
                timecooldownDash = 0;
                }

                break;

            case "ATK_MELEE_BITE":
                durationBite -= Time.deltaTime;
                animator.SetBool("atk1", false);
                if (durationBite <= 0)
                {
                    biteCollider.enabled = false;
                    headCollider.enabled = true;                
                if (IsGrounded)
                {
                    state = "IDLE";
                }
                else
                {
                    state = "JUMP";
                }
                }


                break;

            case "ATK_MELEE_TAIL":
                durationTail -= Time.deltaTime;
                animator.SetBool("atk2", false);
                if (durationTail <= 0)
                {
                    foreach (var collider in tailCollider)
                    {
                        collider.enabled = false;
                    }

                    if (IsGrounded)
                    {
                        state = "IDLE";
                    }
                    else
                    {
                        state = "JUMP";
                    }
                    Debug.Log("Stop AttackTail");
                }
                break;


            case "SMELL":
                durationSmell -= Time.deltaTime;
                animator.SetBool("smelll", false);
                if (durationSmell <= 0)
                {
                    Debug.Log("HERE");
                    state = "IDLE";                    
                }
                break;

            case "ATK_DISTANCE_FIREBALL":
                //CREATION DE LA BOULE DE FEU

                if (IsGrounded)
                {
                    state = "IDLE";
                }
                else
                {
                    state = "JUMP";
                }
                break;

        }

        #endregion StateManager

    }

    private void isIdle()
    {
        if (IsGrounded && state != "DASH" && state != "ATK_MELEE_BITE" && state != "ATK_MELEE_TAIL" && state != "ATK_DISTANCE_FIREBALL" && state != "SMELL")
        {
            state = "IDLE";
        }
         
    }

    #region Dash

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

    #endregion Dash


}
