using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Def : MonoBehaviour
{

    Text defence;

    // Use this for initialization
    void Start()
    {
        defence = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        defence.text = "Defence: " + GameController.instance.players[GameController.instance.currentPlayerIndex].defence;
    }
}