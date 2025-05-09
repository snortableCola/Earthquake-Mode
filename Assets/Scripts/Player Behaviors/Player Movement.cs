using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float _inputConfidenceThreshold = 0.5f;

	[SerializeField] private float _movementTime = 0.3f;
	[SerializeField] private float _jumpHeight = 4f;

	[SerializeField] private Transform _visualMesh;

	private InputAction _motionInput, _interactionInput;
	private Player _player;
	private int _distance;

	private Space _currentSpace;
	private readonly List<Space> _movementPath = new();
	private Space[] _possibleDestinations = new Space[0];
	private PlayerInput _playerInput;
    private void Awake()
	{
		_player = GetComponent<Player>();
		// playerinputs are created by the join scene, one for each player that joins
		var playerInputs = FindObjectsByType<PlayerInput>(FindObjectsInactive.Include, FindObjectsSortMode.None);
		if(playerInputs.Length > _player.PlayerIndex)
		{
            _playerInput = playerInputs.FirstOrDefault(input => input.playerIndex == _player.PlayerIndex);
            _motionInput = _playerInput.actions["2D Motion"];
            _interactionInput = _playerInput.actions["Space Interaction"];
        }
		else 
		{
			gameObject.SetActive(false); 
		}
    }

	/// <summary>
	/// Resets the player's movement, starting their path at their current space.
	/// </summary>
	public void ResetMovementPath()
	{
		_currentSpace = GetComponentInParent<Space>();
		_movementPath.Clear();
		_movementPath.Add(_currentSpace);

		foreach (Space space in _possibleDestinations)
		{
			space.HighlightTag.State = false;
		}
		_possibleDestinations = DestinationFinder.Instance.GetPossibleDestinations(_currentSpace, _distance);
		foreach (Space space in _possibleDestinations)
		{
			space.HighlightTag.State = true;
		}

		DrawNewArrows();
	}

	/// <summary>
	/// Coroutine that encompasses the movement phase of a player's turn, allowing them to move on the board and select a final space.
	/// </summary>
	/// <param name="distance">The distance the player must move on their turn.</param>
	/// <returns>An IEnumerator Coroutine encompassing the player's movement phase.</returns>
	public IEnumerator MovementPhaseCoroutine(int distance, TMP_Text distanceText)
	{
		_distance = distance;

		distanceText.gameObject.SetActive(true);
		distanceText.text = _distance.ToString();

		ResetMovementPath();

		// Loops continuously over many frames until the player chooses to end their movement
		while (true)
		{
			// Process interaction attempts BEFORE movement attempts
			if (_interactionInput.triggered && !EventSystem.current.IsPointerOverGameObject())
			{
				// If the player has travelled the full distance, they may end their movement
				if (_distance == 0) break;

				// The player may interact with non-turn-ending (shop & transport) spaces at any point
				// This cannot be done at the start of the player's path (meaning, just after they've used the space)
				if (_currentSpace.Behavior.HasPassingBehavior && !_currentSpace.BurningTag.State && _movementPath.Count > 1)
				{
					MovementArrowManager.Instance.EraseArrows();
					yield return _currentSpace.Behavior.RespondToPlayerPassing(_player);

					_movementPath.RemoveRange(0, _movementPath.Count - 1); // Prevents player from moving backwards from the space they just used
					DrawNewArrows();
				}
			}

			// If no motion input detected, wait until the next frame
			if (!_motionInput.IsPressed())
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
			if (inputConfidence < _inputConfidenceThreshold || (movingForward ? _distance == 0 : (_movementPath.Count < 2 || targetSpace != _movementPath[^2])))
			{
				yield return null;
				continue;
			}

			// Modifies movement path & remaining distance
			if (movingForward)
			{
				_distance--;
				_movementPath.Add(targetSpace);
			}
			else
			{
				_distance++;
				_movementPath.RemoveAt(_movementPath.Count - 1);
			}

			MovementArrowManager.Instance.EraseArrows();
			// Moves the player to the decided space
			yield return MoveToSpaceCoroutine(targetSpace);
			_currentSpace = targetSpace;
			distanceText.text = _distance.ToString();
			DrawNewArrows();
		}

		foreach (Space space in _possibleDestinations)
		{
			space.HighlightTag.State = false;
		}

		distanceText.gameObject.SetActive(false);
		MovementArrowManager.Instance.EraseArrows();
	}

	/// <summary>
	/// Determines what space the player is trying to move to from their current space by reading their motion input.
	/// </summary>
	/// <param name="forwards">If true, the space is forwards on the player's path. If false, it's behind the player.</param>
	/// <param name="confidence">Represents the similarity between the player's input and the actual direction to their selected space.</param>
	/// <returns>The space that the player is most likely trying to move to.</returns>
	private Space GetSelectedAdjacentSpace(out bool forwards, out float confidence)
	{
		Vector2 inputDirection = _motionInput.ReadValue<Vector2>();
		
        List<Adjacency> adjacentSpaces = AdjacencyManager.Instance.Adjacencies[_currentSpace];

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

		Quaternion startRotation = _visualMesh.rotation;
		Vector2 avgNextDirection = Vector2.zero;
		foreach (Adjacency adj in AdjacencyManager.Instance.Adjacencies[targetSpace])
		{
			if (!adj.IsForwards) continue;
			Vector2 flat = adj.Direction;
			avgNextDirection += flat;
		}
		float angle = Mathf.Atan2(-avgNextDirection.y, avgNextDirection.x) * Mathf.Rad2Deg + 90;
		Quaternion endDirection = Quaternion.Euler(0, angle, 0);

		float timeElapsed = 0f;
		while (timeElapsed < _movementTime)
		{
			timeElapsed += Time.deltaTime; // Movement speed is independent of framerate

			float movementProgress = timeElapsed / _movementTime;
			Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, movementProgress);
			currentPosition.y += _jumpHeight * movementProgress * (1 - movementProgress); // Player jumps follow a quadratic trajectory

			transform.position = currentPosition;
			_visualMesh.rotation = Quaternion.Slerp(startRotation, endDirection, movementProgress);

			yield return null; // Movement iterates frame-by-frame
		}

		transform.position = endPosition;
		_visualMesh.rotation = endDirection;
		transform.SetParent(targetSpace.transform);
	}

	private void DrawNewArrows()
	{
		if (_movementPath.Count > 1)
		{
			MovementArrowManager.Instance.DrawArrow(_movementPath[^2], _currentSpace, false);
		}
		if (_distance == 0) return;
		List<Adjacency> adjacentSpaces = AdjacencyManager.Instance.Adjacencies[_currentSpace];
		foreach (Adjacency adjacency in adjacentSpaces)
		{
			if (adjacency.IsForwards)
			{
				MovementArrowManager.Instance.DrawArrow(_currentSpace, adjacency.Space, true);
			}
		}
	}
}
