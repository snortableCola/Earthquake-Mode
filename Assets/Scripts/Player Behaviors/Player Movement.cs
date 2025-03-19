using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float _inputConfidenceThreshold = 0.5f;

	[SerializeField] private float _movementTime = 0.3f;
	[SerializeField] private float _jumpHeight = 4f;

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

	/// <summary>
	/// Resets the player's movement, starting their path at their current space.
	/// </summary>
	public void ResetMovementPath()
	{
		_space = GetComponentInParent<Space>();
		_path.Clear();
		_path.Add(_space);
	}

	/// <summary>
	/// Coroutine that encompasses the movement phase of a player's turn, allowing them to move on the board and select a final space.
	/// </summary>
	/// <param name="distance">The distance the player must move on their turn.</param>
	/// <returns>An IEnumerator Coroutine encompassing the player's movement phase.</returns>
	public IEnumerator MovementPhaseCoroutine(int distance)
	{
		ResetMovementPath();

		// Loops continuously over many frames until the player chooses to end their movement
		while (true)
		{
			// Process interaction attempts BEFORE movement attempts
			if (_spaceInteraction.triggered)
			{
				// If the player has travelled the full distance, they may end their movement
				if (distance == 0) yield break;

				// The player may interact with non-turn-ending (shop & transport) spaces at any point
				// This cannot be done at the start of the player's path (meaning, just after they've used the space)
				if (!_space.Behavior.EndsTurn && !_space.BurningTag.State && _path.Count > 1)
				{
					yield return _space.Behavior.RespondToPlayer(_player);

					_path.RemoveRange(0, _path.Count - 1); // Prevents player from moving backwards from the space they just used
				}
			}

			// If no motion input detected, wait until the next frame
			if (!_motion2D.IsPressed())
			{
				yield return null;
				continue;
			}

			// Access player's motion input and determine what space they're trying to move to
			Space targetSpace = GetSelectedAdjacentSpace(out bool movingForward, out float inputConfidence);

			// If the player's motion input is invalid, wait until the next frame
			// Motion input is invalid if:
			// - the player's input isn't pointing directly enough towards a space
			// - the player is trying to move forward, but they've already expended their movement distance
			// - the player is trying to move backward to a space that wasn't their previous space
			if (inputConfidence < _inputConfidenceThreshold || (movingForward ? distance == 0 : (_path.Count < 2 || targetSpace != _path[^2])))
			{
				yield return null;
				continue;
			}

			// Modifies movement path & remaining distance (non-turn-ending spaces don't expend distance)
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

			// Moves the player to the decided space
			yield return MoveToSpaceCoroutine(targetSpace);
			_space = targetSpace;
			Debug.Log($"Remaining distance {distance}");
		}
	}

	/// <summary>
	/// Determines what space the player is trying to move to from their current space by reading their motion input.
	/// </summary>
	/// <param name="forwards">If true, the space is forwards on the player's path. If false, it's behind the player.</param>
	/// <param name="confidence">Represents the similarity between the player's input and the actual direction to their selected space.</param>
	/// <returns>The space that the player is most likely trying to move to.</returns>
	private Space GetSelectedAdjacentSpace(out bool forwards, out float confidence)
	{
		Vector2 inputDirection = _motion2D.ReadValue<Vector2>();
		List<Adjacency> adjacentSpaces = AdjacencyManager.Instance.Adjacencies[_space];

		// Determines whichever adjacent space has the maximum similarity with the player's input, using the dot product
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

	/// <summary>
	/// Coroutine for moving the player to a space.
	/// </summary>
	/// <param name="targetSpace">The space for the player to move to.</param>
	/// <returns>An IEnumerator Coroutine for moving the player.</returns>
	public IEnumerator MoveToSpaceCoroutine(Space targetSpace)
	{
		Vector3 startPosition = transform.position;
		Vector3 endPosition = targetSpace.transform.TransformPoint(transform.localPosition); // Ensures the player's located identically relative to its space after landing

		float timeElapsed = 0f;
		while (timeElapsed < _movementTime)
		{
			timeElapsed += Time.deltaTime; // Movement speed is independent of framerate

			float movementProgress = timeElapsed / _movementTime;
			Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, movementProgress);
			currentPosition.y += _jumpHeight * movementProgress * (1 - movementProgress); // Player jumps follow a quadratic trajectory

			transform.position = currentPosition;

			yield return null; // Movement iterates frame-by-frame
		}

		transform.position = endPosition;
		transform.SetParent(targetSpace.transform);
	}
}
