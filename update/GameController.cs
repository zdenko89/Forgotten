using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public GameObject TilePrefab;
    public GameObject UserPlayerPrefab;
    public GameObject AIPrefab;

    public int mapSizeX = 10;
    public int mapSizeY = 10;

    List<List<Tile>> map = new List<List<Tile>>();
    public List<Player> players = new List<Player>();
    public int currentPlayerIndex = 0;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        generateMap();
        generatePlayers();
        DamageTextControl.Initialize();
	}

    void OnGUI()
    {
        players[currentPlayerIndex].TurnOnGUI();
    }

	// Update is called once per frame
	void Update () {
        players[currentPlayerIndex].TurnUpdate();
        
	}

    public void nextTurn()
    {
        if (currentPlayerIndex + 1 < players.Count)
        {
            currentPlayerIndex++;
        }
        else
        {
            currentPlayerIndex = 0;
        }
    }

    //move player
    public void moveCurrentPlayer(Tile destTile)
    {
        players[currentPlayerIndex].gridPosition = destTile.gridPosition;
        players[currentPlayerIndex].moveDestination = destTile.transform.position - Vector3.forward;
    }

    //Attack
    public void attackWithCurrentPlayer(Tile destTile)
    {
        Player target = null;
        foreach (Player p in players) {
            if (p.gridPosition == destTile.gridPosition) {
                target = p;
            }
        }
        if (target != null) {

            players[currentPlayerIndex].Energy-=50;
            int amountOfDamage = (int)Mathf.Floor(players[currentPlayerIndex].damageBase);
            
                target.HP -= amountOfDamage;
                DamageTextControl.CreateDamageText(amountOfDamage.ToString(), transform);
                Debug.Log(players[currentPlayerIndex].playerName + " hit " + target.playerName + " for " + amountOfDamage + " damage");
        }
    }


    //Create the Grid
    void generateMap() {
        map = new List<List<Tile>>();
        for (int i = 0; i < mapSizeX; i++) {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < mapSizeY; j++) {
                Tile tile = ((GameObject)Instantiate(TilePrefab, new Vector3(i - Mathf.Floor(mapSizeX/2), -j + Mathf.Floor(mapSizeY/2), -1f), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
                tile.gridPosition = new Vector2(i, j);
                row.Add(tile);
            }
            map.Add(row);
        }
    }

    //create the players
    void generatePlayers() {
        UserPlayer player;

        //Set position of player 1
        player = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(0, -5, -1.5f), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
        player.gridPosition = new Vector2(0, 0);
        player.playerName = "Iris";
        player.HP = 100;
        player.damageBase = Random.Range(40, 80);
        player.defence = 40;
    player.Energy = 100;

        players.Add(player);

        //Set position of player 2
        player = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(2, 2, -1.5f), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
        player.gridPosition = new Vector2(2, 2);
        player.playerName = "Noctis";
        player.HP = 200;
        player.damageBase = Random.Range(20, 50);
        player.defence = 20;
        player.Energy = 100;

        players.Add(player);

        //Set position of player 3
        player = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(-9, -5, -1.5f), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
        player.gridPosition = new Vector2(-9, -5);
        player.playerName = "Aranea";
        player.HP = 150;
        player.damageBase = Random.Range(20, 50);
        player.defence = 30;
        player.Energy = 100;

        players.Add(player);

        //ADD AI PLAYERS AND OTHER PLAYERS THE SAME WAY 
        // players.Add(aiplayer);
    }
}
