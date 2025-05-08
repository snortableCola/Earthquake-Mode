using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class MultiplayerUIInputTest : MonoBehaviour
{
    [SerializeField] InputSystemUIInputModule module;
    [SerializeField] PlayerInput activePlayer;
    [SerializeField] MultiplayerEventSystem eventSystem;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject LookButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //activePlayer = GetComponentInParent<PlayerInput>();
        //StartCoroutine(TestCoroutine());
    }
    public void RunTest()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.all[0].buttonSouth.wasPressedThisFrame)
        {
            eventSystem = FindFirstObjectByType<MultiplayerEventSystem>();
            module = FindFirstObjectByType<InputSystemUIInputModule>();
            //eventSystem.currentInputModule = module;
            eventSystem.playerRoot = canvas;
            eventSystem.firstSelectedGameObject = LookButton;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            eventSystem = FindFirstObjectByType<MultiplayerEventSystem>();
            module = FindFirstObjectByType<InputSystemUIInputModule>();

            //eventSystem.currentInputModule = module;
            eventSystem.playerRoot = canvas;
            eventSystem.firstSelectedGameObject = LookButton;
        }
    }

    private IEnumerator TestCoroutine()
    {
        yield return new WaitForSeconds(3f);
        eventSystem = FindFirstObjectByType<MultiplayerEventSystem>();
        module = FindFirstObjectByType<InputSystemUIInputModule>();
        //eventSystem.currentInputModule = module;
        eventSystem.playerRoot = canvas;
        eventSystem.firstSelectedGameObject = LookButton;
    }
}
