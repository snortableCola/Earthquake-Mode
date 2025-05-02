using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// JoinScreen handles the player joining process in a multiplayer game.
/// </summary>
public class JoinScreen : MonoBehaviour
{
    [SerializeField] GameObject[] playerIcons = new GameObject[4];
    private PlayerInputManager playerInputManager;

    public GameObject startbutton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInputManager = PlayerInputManager.instance;
    }

    /// <summary>
    /// Called by playerInputManager built in Unity component when a player joins the game.
    /// </summary>
    /// <param name="input"></param>
    private void OnPlayerJoined(PlayerInput input)
    {

        playerIcons[input.playerIndex].SetActive(true);
        input.gameObject.name= $"Player input clone {input.playerIndex + 1}";
        //Debug.Log($"Player joined: {input.playerIndex + 1}");
    }

    public void StartButtonClicked()
    {
        SceneManager.LoadScene("Board");

    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= OnPlayerJoined;
    }
}
