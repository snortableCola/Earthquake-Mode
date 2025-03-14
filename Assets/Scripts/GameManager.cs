using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Player[] _players;

	private int _currentPlayerIndex;
	private int _roundNumber = 1;

	public UnityEvent<Player> TurnPassed = new();
	public UnityEvent RoundPassed = new();

	[ContextMenu("Move Next Player")]
	private void MoveNextPlayer() => StartCoroutine(NextPlayerTurnCoroutine());

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
