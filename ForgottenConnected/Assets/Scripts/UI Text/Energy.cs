using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Energy : MonoBehaviour
{

    Text energy;

    // Use this for initialization
    void Start()
    {
        energy = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        energy.text = "Energy: " + GameController.instance.players[GameController.instance.currentPlayerIndex].Energy;
    }
}
