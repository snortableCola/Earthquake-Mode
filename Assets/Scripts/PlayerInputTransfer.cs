using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputTransfer : MonoBehaviour
{
    [SerializeField]
    List<Player> players;

    // Start is called once before the first execution of Update after the MonoBehaviour is created  
    void Start()
    {
        // Find all Player components in the scene and populate the players list
        players = new List<Player>(FindObjectsByType<Player>(FindObjectsSortMode.InstanceID));

        var playerInputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.InstanceID);
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
