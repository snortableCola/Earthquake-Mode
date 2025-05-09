using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

	public static GameManager Instance { get; private set; }
	public Player[] Players => _players;
	public Player CurrentPlayer { get; private set; }
    public int CurrentPlayerIdx { get; private set; }
	public Space[] Spaces { get; private set; }

	[SerializeField] private int _totalRounds = 10;
	[SerializeField] private Button _diceRollButton;
	[SerializeField] private Button _useItemButton;
	[SerializeField] private GameObject playerProfiles;
	[SerializeField] private TMP_Text _useItemButtonText;
	[SerializeField] private TMP_Text _distanceText;   
	[SerializeField] private Player[] _players;

	[SerializeField] private Image turnHUD;
	[SerializeField] private TMP_Text turnText;
	[SerializeField] Button diceButton;
    public float hudMessageDuration = 3f;
    private bool isTurnHudActive = false;

    private int _roundNumber;
	private bool _diceRolled, _itemUsed;

    [Header("Player Turn UI")]
    [SerializeField] private GameObject[] playerTurnIcons;

    //private List<PlayerInput> playerInputs;

    private void Awake()
	{
		Instance = this;
		Spaces = FindObjectsByType<Space>(FindObjectsSortMode.None);
		_diceRollButton.onClick.AddListener(RespondToDiceRoll);
		_useItemButton.onClick.AddListener(RespondToUseItem);
		playerProfiles.SetActive(false);
		//playerInputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None).ToList();
  //      playerInputs.Sort((input1, input2) => input1.playerIndex.CompareTo(input2.playerIndex));

    }



	private IEnumerator Start() => GameRoundCycle();

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
		
            Debug.Log($"Displaying HUD for TURN {_roundNumber}/{_totalRounds}");
            isTurnHudActive = true;
            turnHUD.gameObject.SetActive(true);
			turnText.gameObject.SetActive(true);
		
			turnText.text = $"TURN {_roundNumber}/{_totalRounds}";
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

	private IEnumerator WaitForShopClose()
	{

		Debug.Log("Checkign to see if shop is open");
		yield return new WaitUntil(() => !ShopPanelManager.Instance.ShopPanel.isShopOpen);
		Debug.Log("shop is closed");

	}
	private IEnumerator GameRoundCycle()
	{
		while (_roundNumber++ < _totalRounds)
		{
			
			yield return WaitForMinigameToEnd();	
            yield return ShowTurnHud(); 
			yield return DoRound();
            yield return WaitForShopClose();
			CurrentPlayer = Players[0];
            MinigameManager.Instance.StartRandomMultiplayerMinigame();
		}
	}

	private IEnumerator DoRound()
	{
		for (CurrentPlayerIdx = 0; CurrentPlayerIdx < _players.Length; CurrentPlayerIdx++)
		{
			CurrentPlayer = _players[CurrentPlayerIdx];
			if (CurrentPlayer.multiplayerEventSystem == null) continue;
			for (int i = 0; i < _players.Length; i++)
			{
				MultiplayerEventSystem eventSystem = _players[i].multiplayerEventSystem;
				if (eventSystem == null) continue;
				eventSystem.gameObject.SetActive(i == CurrentPlayerIdx);
			}
			yield return DoPlayerTurn();
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
        ShowPlayerTurnIcon(CurrentPlayer.PlayerIndex);

        //set the player's multiplayerEventsystem firstselected property to roll dice
        CurrentPlayer.multiplayerEventSystem.firstSelectedGameObject = diceButton.gameObject;
        Debug.Log($"[DoPlayerTurn] Player {CurrentPlayer.name}'s turn starting.");
        DisasterManager.Instance.SetCurrentPlayer(CurrentPlayer);
		DisasterManager.Instance.UpdateDisasterInfo();
        CurrentPlayer.UsedItem = null;

		if (CurrentPlayer != null && CurrentPlayer.FrozenTag.State)
		{
			Debug.Log($"{CurrentPlayer.name} was frozen and passed its turn.");
			CurrentPlayer.FrozenTag.State = false; // Player is unfrozen after their turn would've been skipped
			yield break;
		}

		PanelManager.Instance.ShowMovementUI();
		yield return WaitForDiceRoll();

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
			yield return FireSpace.Instance.RespondToPlayerEnd(CurrentPlayer);
		}
		else
		{
			yield return endingSpace.Behavior.RespondToPlayerEnd(CurrentPlayer);
		}
       

        DisasterManager.Instance.Wildfire.TryFireProgress(CurrentPlayer);
        HideAllPlayerTurnIcons();
    }
    private void ShowPlayerTurnIcon(int playerIndex)
    {
        // Ensure all icons are hidden first
        HideAllPlayerTurnIcons();

        // Show the icon for the current player
        if (playerIndex >= 0 && playerIndex < playerTurnIcons.Length)
        {
            playerTurnIcons[playerIndex].gameObject.SetActive(true);
			Debug.Log($"Showing turn icon for player {playerIndex}");
        }
        else
        {
            Debug.LogError("Player index is out of range for playerTurnIcons.");
        }
    }

    private void HideAllPlayerTurnIcons()
    {
        foreach (var icon in playerTurnIcons)
        {
            icon.gameObject.SetActive(false);
        }
    }
    private void RespondToDiceRoll() => _diceRolled = true;
	private void RespondToUseItem() => _itemUsed = true;

	private IEnumerator WaitForDiceRoll()
	{
		bool hasItem = CurrentPlayer.HeldItem != null;
		_useItemButton.interactable = hasItem;
		_useItemButtonText.text = $"Use {(hasItem ? CurrentPlayer.HeldItem.name : "Item")}";

		_diceRollButton.gameObject.SetActive(true);
		_useItemButton.gameObject.SetActive(true);
		playerProfiles.gameObject.SetActive(true);
		HideAllPlayerTurnIcons();

		yield return new WaitUntil(() => _diceRolled || _itemUsed);

		if (_diceRolled)
		{
			ClearAfterDiceRoll();
			yield break;
		}

		_useItemButton.interactable = false;
		_itemUsed = false;

		PanelManager.Instance.ShowPanel(null, null);
		yield return CurrentPlayer.HeldItem.BeUsedBy(CurrentPlayer);
		PanelManager.Instance.ShowMovementUI();
		CurrentPlayer.HeldItem = null;

		yield return new WaitUntil(() => _diceRolled);
		ClearAfterDiceRoll();

		void ClearAfterDiceRoll()
		{
			_diceRollButton.gameObject.SetActive(false);
			_useItemButton.gameObject.SetActive(false);
			playerProfiles.gameObject.SetActive(false);
			_diceRolled = false;
		}
	}
}
