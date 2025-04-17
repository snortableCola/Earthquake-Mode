using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayer : MonoBehaviour
{
    private PlayerInput playerInput;

    public bool readyToStart = false;
    public void OnSelect()
    {
        Debug.Log($"Player {playerInput?.playerIndex} pressed select");
        readyToStart = true;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
