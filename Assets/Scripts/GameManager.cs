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
	private void MoveNextPlayer()
	{
		Player player = _players[_currentPlayerIndex];
		player.Move();

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
