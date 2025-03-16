using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Player[] _players;

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

		if (player.FrozenTag.State)
		{
			Debug.Log($"{player.name} was frozen and passed its turn.");
			player.FrozenTag.State = false; // Player is unfrozen after their turn would've been skipped
		}
		else
		{
			int distance = Random.Range(1, 11);
			Debug.Log($"{player.name} moves for {distance} spaces.");

			yield return player.Movement.MovementCoroutine(distance);
		}

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
