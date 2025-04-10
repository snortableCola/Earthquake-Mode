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

	[SerializeField] private Image turnHUD;
	[SerializeField] private TMP_Text turnText;
    public float hudMessageDuration = 3f;
    private bool isTurnHudActive = false;
    [SerializeField] private NegativeSpace negativeSpaceScript; // Reference to NegativeSpace script
    [SerializeField] private ResourceSpace resourceSpaceScript; // Reference to ResourceSpace script

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

	private IEnumerator ShowTurnHud()
	{
		_roundNumber.ToString();
        if (isTurnHudActive)
        {
            Debug.LogWarning("[ShowTurnHud] HUD is already active. Skipping.");
            yield break;
        }
        if ( turnHUD != null && turnText != null)
		{
		
            Debug.Log($"Displaying HUD for TURN {_roundNumber}/10");
            isTurnHudActive = true;
            turnHUD.gameObject.SetActive(true);
			turnText.gameObject.SetActive(true);
		
			turnText.text = $"TURN {_roundNumber}/10";
            yield return new WaitForSeconds(hudMessageDuration);
            turnHUD.gameObject.SetActive(false);
            turnText.gameObject.SetActive(false);
			isTurnHudActive = false; 
        }
	
	}
  
    private IEnumerator WaitForMinigameToEnd()
    {
        // Wait for the minigame sequence to finish
        Debug.Log("[WaitForMinigameToEnd] Waiting for minigame to finish...");
        yield return new WaitUntil(() => !MinigameManager.Instance.IsMinigameSequenceOngoing);
        Debug.Log("[WaitForMinigameToEnd] Minigame sequence finished.");
    }


    private IEnumerator GameRoundCycle()
	{
		while (_roundNumber++ < _totalRounds)
		{
			
			yield return WaitForMinigameToEnd();	
            yield return ShowTurnHud(); 
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
            yield return WaitForLastPlayerHudCompletion();
        }
	}
    private IEnumerator WaitForLastPlayerHudCompletion()
    {
        // Wait for the last player's HUD messages (if any) to finish
        Space endingSpace = CurrentPlayer.GetComponentInParent<Space>();
        if (endingSpace != null && endingSpace.Behavior != null)
        {
            Debug.Log("[WaitForLastPlayerHudCompletion] Waiting for last player's HUD messages to finish...");
            yield return endingSpace.Behavior.WaitForHudCompletion();
        }
    }
    private IEnumerator DoPlayerTurn()
	{
        Debug.Log($"[DoPlayerTurn] Player {CurrentPlayer.name}'s turn starting.");
        DisasterManager.Instance.SetCurrentPlayer(CurrentPlayer);
		DisasterManager.Instance.UpdateDisasterInfo();
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
        if (endingSpace != null)
        {
            Debug.Log($"[DoPlayerTurn] Player landed on {endingSpace.name}. Responding to space behavior.");
            yield return endingSpace.Behavior.RespondToPlayer(CurrentPlayer);
        }
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
