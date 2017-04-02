using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Slider healthBar; // this will be the variable for the health bar
    public Text HPText; // this will be the text displayed under the bar
    public PlayerHealthManager playerHealth; // controlling the player health


    // Use this for initialization
    void Start()
    {

    }
	// Update is called once per frame
	void Update () {
        healthBar.maxValue = playerHealth.playerMaxHealth; // on start/begginging of the player spawning the payer will start with maximum amount of health
        healthBar.value = playerHealth.playerCurrentHealth; // on start/beginning  the health bar will start with max value
        HPText.text = "HP:  " + playerHealth.playerCurrentHealth + "/" + playerHealth.playerMaxHealth; // this will write the actual health of the player next to HP
	}
}
