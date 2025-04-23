using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoinScreen : MonoBehaviour
{
    [SerializeField] GameObject[] playerIcons= new GameObject[4];
    private PlayerInputManager playerInputManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInputManager = FindFirstObjectByType<PlayerInputManager>();
        
    }

    private void OnPlayerJoined(PlayerInput input)
    {
        playerIcons[input.playerIndex].SetActive(true);
        input.gameObject.name= $"Player input clone {input.playerIndex + 1}";
        Debug.Log($"Player joined: {input.playerIndex + 1}");
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
