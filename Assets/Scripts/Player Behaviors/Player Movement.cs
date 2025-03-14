using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	InputAction input2D;
	public void Awake()
	{
		input2D = InputSystem.actions.FindAction("2D Movement");
		Vector2 input = input2D.ReadValue<Vector2>();
		
	}

	public void StartListening()
	{

	}
}
