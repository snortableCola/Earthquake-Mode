using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputTransfer : MonoBehaviour
{
    [SerializeField]
    List<Player> players;

    // Start is called once before the first execution of Update after the MonoBehaviour is created  
    private void Awake()
    {
        players = new List<Player>(FindObjectsByType<Player>(FindObjectsSortMode.None));
        players.Sort((player1, player2) => player1.PlayerIndex.CompareTo(player2.PlayerIndex));
        
    }
    void Start()
    {

        List<PlayerInput> playerInputs = new List<PlayerInput>(FindObjectsByType<PlayerInput>(FindObjectsSortMode.None));
        playerInputs.Sort((input1, input2) => input1.playerIndex.CompareTo(input2.playerIndex));

        int playerIndex = 0;

        foreach (var playerInput in playerInputs)
        {
            if (playerIndex < players.Count)
            {
                playerInput.transform.SetParent(players[playerIndex].transform);
                playerIndex++;
            }
            else
            {
                Debug.LogWarning("More PlayerInput instances than Player objects. Some PlayerInput instances will not have parents.");
                break;
            }
        }
    }

    // Update is called once per frame  
    void Update()
    {

    }
}
