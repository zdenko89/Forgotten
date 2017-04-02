using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayer : MonoBehaviour {

    public int damageToGive;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void onCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Player") // whatever this script is attached and collides with player it then will....
        {
            other.gameObject.GetComponent<PlayerHealthManager>().HurtPlayer(damageToGive); // damage the player
        }
    }
}
