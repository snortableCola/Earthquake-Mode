using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputTransfer : MonoBehaviour
{
    public PlayerInputTransfer Instance { get; private set; }

    [SerializeField] public List<Player> players;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;

            players = new List<Player>(FindObjectsByType<Player>(FindObjectsSortMode.None));
            players.Sort((player1, player2) => player1.PlayerIndex.CompareTo(player2.PlayerIndex));
            List<PlayerInput> playerInputs = new List<PlayerInput>(FindObjectsByType<PlayerInput>(FindObjectsSortMode.None));
            playerInputs.Sort((input1, input2) => input1.playerIndex.CompareTo(input2.playerIndex));
        
            foreach (var playerInput in playerInputs)
            {
                int _playerIndex = playerInput.playerIndex;
                if (_playerIndex < players.Count)
                {
                    playerInput.transform.SetParent(players[_playerIndex].transform);
                }
                else
                {
                    Debug.LogWarning("More PlayerInput instances than Player objects. Some PlayerInput instances will not have parents.");
                    break;
                }
            }
        }
    }
}
