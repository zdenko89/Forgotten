using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour {

    public Vector2 gridPosition = Vector2.zero;
    public Vector3 moveDestination;
    public float moveSpeed = 5.0f;
    public string playerName = " ";

    public bool moving = false;
    public bool attacking = false;

    //Player stats
    public int HP = 100;
    public int Energy = 100;
    public float defence = 20;
    public float damageBase = 50;

   // Text text;

    void Awake()
    {
        moveDestination = transform.position;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    }

    public virtual void TurnUpdate()
    {
        if (Energy <= 0)
        {
            Energy = 100;
            moving = false;
            attacking = false;
            GameController.instance.nextTurn();
        }

    }

    public virtual void TurnOnGUI()
    {

    }
}
