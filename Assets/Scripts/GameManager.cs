using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	public Player[] Players => _players;
	public Player CurrentPlayer { get; private set; }
	public Space[] Spaces { get; private set; }

	[SerializeField] private int _totalRounds = 10;
	[SerializeField] private Button _diceRollButton;
	[SerializeField] private TMP_Text _distanceText;   
	[SerializeField] private Player[] _players;

	private int _roundNumber;
	private bool _diceRolled;

	private void Awake()
	{
		Instance = this;
		Spaces = FindObjectsByType<Space>(FindObjectsSortMode.None);
		_diceRollButton.onClick.AddListener(RespondToDiceRoll);
	}

	private IEnumerator Start() => GameRoundCycle();


	public readonly UnityEvent RoundPassed = new();
	private IEnumerator GameRoundCycle()
	{
		while (_roundNumber++ < _totalRounds)
		{
			yield return DoRound();
			RoundPassed.Invoke();
		}
	}

	public readonly UnityEvent<Player> TurnPassed = new();
	private IEnumerator DoRound()
	{
		for (int playerIdx = 0; playerIdx < _players.Length; playerIdx++)
		{
			CurrentPlayer = _players[playerIdx];
			yield return DoPlayerTurn();
			TurnPassed.Invoke(CurrentPlayer);
		}
	}

	private IEnumerator DoPlayerTurn()
	{
		CurrentPlayer.UsedItem = null;

		if (CurrentPlayer.FrozenTag.State)
		{
			Debug.Log($"{CurrentPlayer.name} was frozen and passed its turn.");
			CurrentPlayer.FrozenTag.State = false; // Player is unfrozen after their turn would've been skipped
			yield break;
		}

		_diceRollButton.gameObject.SetActive(true);
		yield return new WaitUntil(() => _diceRolled);
		_diceRollButton.gameObject.SetActive(false);
		_diceRolled = false;

		int distance = Random.Range(1, 11);
		switch (CurrentPlayer.UsedItem)
		{
			case TravelPlan:
				int newDistance = Random.Range(1, 11);
				if (newDistance > distance) distance = newDistance;
				break;
			case PrivateJet:
				distance += Random.Range(1, 11);
				break;
		}

		CameraManager.Instance.MoveToPlayer(CurrentPlayer);
		
		yield return CurrentPlayer.Movement.MovementPhaseCoroutine(distance, _distanceText);

		CameraManager.Instance.ReturnOverhead();

		Space endingSpace = CurrentPlayer.GetComponentInParent<Space>();
		if (endingSpace.BurningTag.State && CurrentPlayer.UsedItem is not HeliEvac)
		{
			Debug.Log($"{CurrentPlayer} landed on a space which is on fire.");
		}
		else
		{
			yield return endingSpace.Behavior.RespondToPlayer(CurrentPlayer);
		}
	}

	private void RespondToDiceRoll() => _diceRolled = true;
}
