using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    public Vector2 gridPosition = Vector2.zero;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //Highlites the hovered tile
    void OnMouseEnter()
    {
        if (GameController.instance.players[GameController.instance.currentPlayerIndex].moving)
        {
            GetComponent<Renderer>().material.color = Color.green;//highlighted tile is green if free to move
        }
        else if (GameController.instance.players[GameController.instance.currentPlayerIndex].attacking)
        {
            GetComponent<Renderer>().material.color = Color.red;//highlighted tile is red, if there is player on there
        }
    }

    void OnMouseExit() {
        GetComponent<Renderer>().material.color = Color.white;//All other remain white
    }

    void OnMouseDown()
    {
        if (GameController.instance.players[GameController.instance.currentPlayerIndex].moving)
        {
            GameController.instance.moveCurrentPlayer(this);
        }
        else if (GameController.instance.players[GameController.instance.currentPlayerIndex].attacking)
        {
            GameController.instance.attackWithCurrentPlayer(this);
        }
        
    }
}
