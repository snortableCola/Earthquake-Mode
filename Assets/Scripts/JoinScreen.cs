using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoinScreen : MonoBehaviour
{
    private PlayerInputManager playerInputManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInputManager = FindFirstObjectByType<PlayerInputManager>();
        
    }

    private void OnPlayerJoined(PlayerInput input)
    {
        
        Debug.Log($"Player joined: {input.playerIndex}");
       // PlayerList.Add(input.gameObject.GetComponent<PlayerController>());
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= OnPlayerJoined;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
