using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
	private Player[] _players;
	[SerializeField] private Transform _board;

	private void Awake()
	{
		_players = _board.GetComponentsInChildren<Player>();
	}

	private int _currentPlayerIndex;
	private int _roundNumber = 1;

	/// <summary>
	/// Event that triggers whenever a turn is completed, providing the player whose turn passed.
	/// </summary>
	public UnityEvent<Player> TurnPassed = new();

	/// <summary>
	/// Event that triggers whenever a full round of play for all players has completed.
	/// </summary>
	public UnityEvent RoundPassed = new();

	[ContextMenu("Move Next Player")]
	private void MoveNextPlayer() => StartCoroutine(NextPlayerTurnCoroutine());

	/// <summary>
	/// Coroutine representing the next player taking their turn.
	/// </summary>
	/// <returns>Coroutine for handling the next player's turn.</returns>
	private IEnumerator NextPlayerTurnCoroutine()
	{
		Player player = _players[_currentPlayerIndex];
		
		yield return player.RandomMovementCoroutine();

		_currentPlayerIndex++;

		TurnPassed.Invoke(player);

		if (_currentPlayerIndex == _players.Length)
		{
			_currentPlayerIndex = 0;

			Debug.Log($"Completed round {_roundNumber}");
			_roundNumber++;
			RoundPassed.Invoke();
		}
	}
}
