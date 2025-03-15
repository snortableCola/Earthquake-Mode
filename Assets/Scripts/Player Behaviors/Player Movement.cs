using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private AdjacencyManager _adjacencyManager;

	private InputAction _motion2D, _spaceInteraction;
	private Player _player;

	private void Awake()
	{
		_motion2D = InputSystem.actions.FindAction("2D Motion");
		_spaceInteraction = InputSystem.actions.FindAction("Space Interaction");

		_player = GetComponent<Player>();
	}

	[ContextMenu("Test With 5")]
	public void TestWith5() => StartCoroutine(MovementCoroutine(5));

	public IEnumerator MovementCoroutine(int distance)
	{
		Space space = _player.GetComponentInParent<Space>();
		List<Space> path = new() { space };

		while (!(_spaceInteraction.triggered && distance == 0))
		{
			if (!_motion2D.IsPressed())
			{
				yield return null;
				continue;
			}

			Vector2 inputDirection = _motion2D.ReadValue<Vector2>();
			Space targetSpace = GetDecidedSpace(space, inputDirection, out bool movingForward, out float inputConfidence);

			if (inputConfidence < 0.5 || (movingForward ? distance == 0 : (path.Count < 2 || targetSpace != path[^2])))
			{
				yield return null;
				continue;
			}

			if (movingForward)
			{
				distance--;
				path.Add(targetSpace);
			}
			else
			{
				distance++;
				path.RemoveAt(path.Count - 1);
			}

			space = targetSpace;
			yield return _player.JumpToSpaceCoroutine(space, false);
			Debug.Log($"Remaining distance {distance}");
		}

		if (space.BurningTag.State)
		{
			Debug.Log($"{_player} landed on a space which is on fire.");
		}
		else if (space.TryGetComponent<SpaceLandedBehavior>(out var behavior))
		{
			behavior.ReactToPlayerLanding(_player);
		}

		yield break;
	}

	private Space GetDecidedSpace(Space origin, Vector2 inputDirection, out bool forwards, out float confidence)
	{
		List<Adjacency> adjacentSpaces = _adjacencyManager.Adjacencies[origin];

		Adjacency mostLikelyChoice = adjacentSpaces[0];
		confidence = Vector2.Dot(inputDirection, mostLikelyChoice.Direction);

		foreach (Adjacency adjacency in adjacentSpaces.Skip(1))
		{
			float accuracy = Vector2.Dot(inputDirection, adjacency.Direction);

			if (accuracy > confidence)
			{
				confidence = accuracy;

				mostLikelyChoice = adjacency;
			}
		}

		forwards = mostLikelyChoice.IsForwards;
		return mostLikelyChoice.Space;
	}
}
