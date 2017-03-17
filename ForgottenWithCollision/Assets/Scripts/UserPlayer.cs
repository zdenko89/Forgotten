using UnityEngine;
using System.Collections;

public class UserPlayer : Player {

    public Sprite Player;
    public Sprite playerSelect;

	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    void Update() {
        if (GameController.instance.players[GameController.instance.currentPlayerIndex] == this)
        {
            transform.GetComponent<SpriteRenderer>().sprite = playerSelect; //if player selected
        }
        else {
            transform.GetComponent<SpriteRenderer>().sprite = Player;//change sprite for all other
        }
        if (HP <= 0)
        {
            Destroy(gameObject); // if health goes to 0, delete sprite
        }
        
       }

    public override void TurnUpdate()
    {
        if (Vector3.Distance(moveDestination, transform.position) > 0.1f)
        {
            transform.position += (moveDestination - transform.position).normalized * moveSpeed * Time.deltaTime; 
            

            //Check if reached destination
            if (Vector3.Distance(moveDestination, transform.position) <= 0.1f)
            {
                transform.position = moveDestination;
                Energy -= 50;
            }
        }
        base.TurnUpdate();
    }


    //GUI 
    public override void TurnOnGUI()
    {
        float buttonHeight = 100;
        float buttonWidth = 250;

        //Move button
        Rect buttonRect = new Rect(0, Screen.height - buttonHeight * 3, buttonWidth, buttonHeight);

        if (GUI.Button(buttonRect, "Move"))
        {
            if (!moving)
            {
                moving = true;
                attacking = false;
            }
            else {
                moving = false;
                attacking = false;
            }
        }

        //Attack button
        buttonRect = new Rect(0, Screen.height - buttonHeight * 2, buttonWidth, buttonHeight);

        if (GUI.Button(buttonRect, "Attack"))
        {
            if (!attacking)
            {
                moving = false;
                attacking = true;
            }
            else {
                moving = false;
                attacking = false;
            }
        }

        //End turn button
        buttonRect = new Rect(0, Screen.height - buttonHeight * 1, buttonWidth, buttonHeight);

        if (GUI.Button(buttonRect, "End Turn"))
        {
            Energy = 100;
            moving = false;
            attacking = false;
            GameController.instance.nextTurn();
        }
        base.TurnOnGUI();
    }
}
