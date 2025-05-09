using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SabotageManager : MonoBehaviour
{
	public static SabotageManager Instance { get; private set; }
	public bool Possible { get; private set; }

	[SerializeField] private int _maxCoinsToSteal;
	[SerializeField] private int _stealingOilCost;

	[SerializeField] private GameObject _victimSelectPanel;
	[SerializeField] private Button[] _victimSelectButtons;
	private int _selectedVictimIndex;

	[SerializeField] private GameObject _methodSelectPanel;
	[SerializeField] private Button _stealCoinsButton;
	[SerializeField] private Button _stealOilButton;
	private bool? _stealingOil;

	[SerializeField] private GameObject _consequencePanel;
	[SerializeField] private Button _exitSabotageButton;
	private bool _exiting;

	private PanelManager _panelManager;

    private void Awake()
	{
		Instance = this;

		_stealCoinsButton.onClick.AddListener(() => _stealingOil = false);
		_stealOilButton.onClick.AddListener(() => _stealingOil = true);

		_exitSabotageButton.onClick.AddListener(() => _exiting = true);
	}

	private void Start()
	{
		int totalPlayers = GameManager.Instance.Players.Length;
		Possible = totalPlayers > 1;
		if (!Possible) return;

		for (int i = 0; i < totalPlayers - 1; i++)
		{
			int buttonIndex = i;
			_victimSelectButtons[i].onClick.AddListener(() => _selectedVictimIndex = buttonIndex);
			_victimSelectButtons[i].interactable = true;
		}

		_victimSelectButtons[3].onClick.AddListener(() => _selectedVictimIndex = 3); // For "Random" button at the end
	}

	public IEnumerator Sabotage(Player saboteur)
	{
		Player[] allPlayers = GameManager.Instance.Players;
		List<Player> otherPlayers = new();
		for (int i = 0; i < allPlayers.Length; i++)
		{
			if (i == GameManager.Instance.CurrentPlayerIdx) continue;
			otherPlayers.Add(allPlayers[i]);
		}
		otherPlayers.Sort();

		// Make buttons show player names
		for (int i = 0; i < otherPlayers.Count; i++)
		{
			_victimSelectButtons[i].gameObject.GetComponentInChildren<TextMeshProUGUI>().text = otherPlayers[i].name;
		}

		// Shows the victim selection panel
		PanelManager.Instance.ShowPanel(_victimSelectPanel, _panelManager._victimSelectButton);

		// Wait for selection (for sake of implementation, this is how to get a random victim)
		_selectedVictimIndex = -1;
		yield return new WaitUntil(() => _selectedVictimIndex != -1);
		if (_selectedVictimIndex == 3) _selectedVictimIndex = Random.Range(0, otherPlayers.Count); // Last button counts as random

		// Use the index of the selected victim button to get an actual victim player object
		Player selectedVictim = otherPlayers[_selectedVictimIndex];

		// Determine whether or not it's possible for the saboteur to steal oil
		bool canStealOil = selectedVictim.Oil > 0 && saboteur.Coins >= _stealingOilCost;
		_stealOilButton.interactable = canStealOil;

		// Display choice between stealing coins or oil
		PanelManager.Instance.ShowPanel(_methodSelectPanel, _panelManager._methodSelectButton);

		// Wait for selection (for this, let's just say if they can steal oil, they will steal oil)
		_stealingOil = null;
		yield return new WaitUntil(() => _stealingOil != null);

		// Handle the logic for taking away oil or coins depending on selection
		if (_stealingOil == true)
		{
			selectedVictim.Oil--;
			saboteur.Oil++;
		}
		else
		{
			int coinsStolen = Mathf.Min(_maxCoinsToSteal, selectedVictim.Coins);
			selectedVictim.Coins -= coinsStolen;
			saboteur.Coins += coinsStolen;
		}

		// Show the sabotage consequence
		PanelManager.Instance.ShowPanel(_consequencePanel, _panelManager._consequenceButton);

		// Wait for consequence to be canceled out of
		_exiting = false;
		yield return new WaitUntil(() => _exiting);
	}
}
