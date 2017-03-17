using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Stats : MonoBehaviour {

    Text Health;

    // Use this for initialization
	void Start () {
        Health = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        Health.text = "Health: " + GameController.instance.players[GameController.instance.currentPlayerIndex].HP;
    }
}
