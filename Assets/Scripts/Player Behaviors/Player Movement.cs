using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float _movementTime;
	[SerializeField] private float _jumpHeight;
	[SerializeField] private AdjacencyManager _adjacencyManager;

	private InputAction _motion2D, _spaceInteraction;
	private Player _player;

	private Space _space;
	private readonly List<Space> _path = new();

	private void Awake()
	{
		_motion2D = InputSystem.actions.FindAction("2D Motion");
		_spaceInteraction = InputSystem.actions.FindAction("Space Interaction");

		_player = GetComponent<Player>();
	}

	public void ResetMovementPath(Space space)
	{
		_space = space;
		_path.Clear();
		_path.Add(space);
	}

	public IEnumerator MovementCoroutine(int distance)
	{
		ResetMovementPath(GetComponentInParent<Space>());

		while (true)
		{
			if (_spaceInteraction.triggered)
			{
				if (distance == 0) yield break;

				if (!_space.Behavior.EndsTurn && !_space.BurningTag.State && _path.Count > 1)
				{
					_path.RemoveRange(0, _path.Count - 1);
					yield return _space.Behavior.RespondToPlayer(_player);
					continue;
				}
			}

			if (!_motion2D.IsPressed())
			{
				yield return null;
				continue;
			}

			Vector2 inputDirection = _motion2D.ReadValue<Vector2>();
			Space targetSpace = GetDecidedSpace(_space, inputDirection, out bool movingForward, out float inputConfidence);

			if (inputConfidence < 0.5 || (movingForward ? distance == 0 : (_path.Count < 2 || targetSpace != _path[^2])))
			{
				yield return null;
				continue;
			}

			if (movingForward)
			{
				if (targetSpace.Behavior.EndsTurn) distance--;
				_path.Add(targetSpace);
			}
			else
			{
				if (_space.Behavior.EndsTurn) distance++;
				_path.RemoveAt(_path.Count - 1);
			}

			yield return JumpToSpaceCoroutine(targetSpace);
			_space = targetSpace;
			Debug.Log($"Remaining distance {distance}");
		}
	}

	private Space GetDecidedSpace(Space origin, Vector2 inputDirection, out bool forwards, out float confidence)
	{
		List<Adjacency> adjacentSpaces = _adjacencyManager.Adjacencies[origin];

		Adjacency selection = adjacentSpaces[0];
		confidence = Vector2.Dot(inputDirection, selection.Direction);

		foreach (Adjacency adjacency in adjacentSpaces.Skip(1))
		{
			float accuracy = Vector2.Dot(inputDirection, adjacency.Direction);

			if (accuracy > confidence)
			{
				confidence = accuracy;
				selection = adjacency;
			}
		}

		forwards = selection.IsForwards;
		return selection.Space;
	}

	public IEnumerator JumpToSpaceCoroutine(Space targetSpace)
	{
		Vector3 startPosition = transform.position;
		Vector3 endPosition = targetSpace.transform.TransformPoint(transform.localPosition); // Ensures the player's located identically relative to its space after landing

		float timeElapsed = 0f;
		while (timeElapsed < _movementTime)
		{
			timeElapsed += Time.deltaTime; // Movement is frame-independent

			float movementProgress = timeElapsed / _movementTime;
			Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, movementProgress);
			currentPosition.y += _jumpHeight * movementProgress * (1 - movementProgress); // Player follows quadratic trajectory

			transform.position = currentPosition;

			yield return null; // Movement iterates frame-by-frame
		}

		transform.position = endPosition;
		transform.SetParent(targetSpace.transform);
	}
}
