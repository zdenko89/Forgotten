using UnityEngine;
using System.Collections;

public class AI : Player {
    


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
            else
            {
                moveDestination = new Vector3(0 - Mathf.Floor(GameController.instance.mapSizeX / 2), 1.5f, -0 + Mathf.Floor(GameController.instance.mapSizeY / 2));
            }
        }
        base.TurnUpdate();
    }

    public override void TurnOnGUI()
    {
        base.TurnOnGUI();
    }
}
