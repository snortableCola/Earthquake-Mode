using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public int playerID;
    [Header("Input Settings")]
    public PlayerInput playerInput;
    private PlayerController focusedPlayerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void SetupPlayer(int newPlayerID)
    {
        playerID = newPlayerID;
        playerInput = GetComponent<PlayerInput>();

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
