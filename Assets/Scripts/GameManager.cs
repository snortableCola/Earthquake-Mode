using UnityEngine;
using UnityEngine.Events;

public class PlayManager : MonoBehaviour
{
	[SerializeField] private Player[] _players;
	[SerializeField] private int _maxRounds;

	private int _currentPlayerIndex;

	public UnityEvent TurnPassed = new();
	public UnityEvent RoundPassed = new();

	[ContextMenu("Move Next Player")]
	private void MoveNextPlayer()
	{
		_players[_currentPlayerIndex].Move();

		_currentPlayerIndex++;

		TurnPassed.Invoke();

		if (_currentPlayerIndex == _players.Length)
		{
			_currentPlayerIndex = 0;
			RoundPassed.Invoke();
		}
	}
}
