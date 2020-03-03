using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : CharacterBehavior
{
    [SerializeField] Rigidbody PlayerRB;
    [SerializeField] int numOfHearts;

    public Image[] Hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private float horizontalMovement;

    // Start is called before the first frame update
    void Start()
    {
        MaxHp = numOfHearts;
        Hp = numOfHearts -1;
        MoveSpeed = 10.0f;
        JumpPower = 500.0f;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Jump")) //Project settings input
        {
            PlayerRB.AddForce(Vector3.up * JumpPower);
            Debug.Log("Jump");
        }

        if (Input.GetButton("Horizontal")) //Project settings input
        {
            horizontalMovement = Input.GetAxisRaw("Horizontal") * MoveSpeed;
            //PlayerRB.velocity = new Vector3(horizontalMovement,0,0);
            Debug.Log("H" + Input.GetAxisRaw("Horizontal"));
            transform.Translate(horizontalMovement*Time.deltaTime,0,0);
        }

        // PARTIE HEALTHPOINT A BOUGER ===============================================================
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
        // PARTIE HEALTHPOINT A BOUGER ===============================================================

    }
}
