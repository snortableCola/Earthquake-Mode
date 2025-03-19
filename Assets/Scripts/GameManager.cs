using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	public Player[] Players => _players;
	public Space[] Spaces { get; private set; }

	[SerializeField] private Player[] _players;


	private int _currentPlayerIndex;
	private int _roundNumber = 1;

	private void Awake()
	{
		Instance = this;
		Spaces = FindObjectsByType<Space>(FindObjectsSortMode.None);
	}

	/// <summary>
	/// Event that triggers whenever a turn is completed, providing the player whose turn passed.
	/// </summary>
	public UnityEvent<Player> TurnPassed = new();

	/// <summary>
	/// Event that triggers whenever a full round of play for all players has completed.
	/// </summary>
	public UnityEvent RoundPassed = new();

	[ContextMenu("Move Next Player")]
	private void DoPlayerTurn() => StartCoroutine(PlayerTurnCoroutine());

	/// <summary>
	/// Coroutine representing the next player taking their turn.
	/// </summary>
	/// <returns>Coroutine for handling the next player's turn.</returns>
	private IEnumerator PlayerTurnCoroutine()
	{
		Player player = _players[_currentPlayerIndex];

		if (player.FrozenTag.State)
		{
			Debug.Log($"{player.name} was frozen and passed its turn.");
			player.FrozenTag.State = false; // Player is unfrozen after their turn would've been skipped

			MoveToNextPlayer();
			yield break;
		}

		int distance = Random.Range(1, 11);
		Debug.Log($"{player.name} moves for {distance} spaces.");

		yield return player.Movement.MovementCoroutine(distance);

		Space endingSpace = player.GetComponentInParent<Space>();
		if (endingSpace.BurningTag.State)
		{
			Debug.Log($"{player} landed on a space which is on fire.");
		}
		else
		{
			yield return endingSpace.Behavior.RespondToPlayer(player);
		}

		MoveToNextPlayer();
	}

	private void MoveToNextPlayer()
	{
		TurnPassed.Invoke(_players[_currentPlayerIndex]);

		_currentPlayerIndex++;

		if (_currentPlayerIndex == _players.Length)
		{
			_currentPlayerIndex = 0;

			Debug.Log($"Completed round {_roundNumber}");
			_roundNumber++;
			RoundPassed.Invoke();
		}
	}
}
