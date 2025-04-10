using UnityEngine;

public class Multiplayer : MonoBehaviour
{
    public GameObject[] players; // Array to hold player objects
    private int currentPlayerIndex = 0; // Index of the current player

    public bool EndTurn;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Ensure only the first player is active at the start
        SetActivePlayer(currentPlayerIndex);
    }

    // Update is called once per frame
    void Update()
    {
        // Check for turn change input (e.g., pressing the space key)
        if (EndTurn = true)
        {
            ChangeTurn();
        }
    }

    // Activates the current player and deactivates others
    private void SetActivePlayer(int index)
    {
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Player>().enabled = (i == index);
        }
    }

    // Changes the turn to the next player
    private void ChangeTurn()
    {
        //if it is player 1, 2, or 3's turn, change to the next player
        if (currentPlayerIndex<= 2) 
        {
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        }
        //revert back to player1
        else
        {
            currentPlayerIndex = 0;
        }
        SetActivePlayer(currentPlayerIndex);
    }
}
