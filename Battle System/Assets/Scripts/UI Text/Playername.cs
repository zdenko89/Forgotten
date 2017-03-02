using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Playername : MonoBehaviour
{

    Text Player;

    // Use this for initialization
    void Start()
    {
        Player = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Player.text = " " + GameController.instance.players[GameController.instance.currentPlayerIndex].playerName;
    }
}
