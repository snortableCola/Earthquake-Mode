using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestJoinScreen : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput input)
    {
        Debug.Log($"Player joined: {input.playerIndex}");
       // PlayerList.Add(input.gameObject.GetComponent<PlayerController>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
