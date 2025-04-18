using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SabotageManager : MonoBehaviour
{
	public static SabotageManager Instance { get; private set; }

	[SerializeField] private GameObject _victimSelectPanel;
	[SerializeField] private GameObject _methodSelectPanel;
	[SerializeField] private GameObject _consequencePanel;

	[SerializeField] private Button[] _playerSelectButtons;
	private int _selectedPlayerIndex;

	[SerializeField] private int MaxCoinsToSteal;
	[SerializeField] private int StealingOilCost;

	private void Awake()
	{
		Instance = this;

		if (_playerSelectButtons.Length != GameManager.Instance.Players.Length)
		{
			Debug.LogError("The sabotage manager doesn't have the correct amount of victim-selection buttons");
		}

		for (int i = 0; i < _playerSelectButtons.Length; i++)
		{
			_playerSelectButtons[i].onClick.AddListener(SetSelectedPlayer);
			void SetSelectedPlayer() => _selectedPlayerIndex = i;
		}
	}

	public IEnumerator Sabotage(Player saboteur)
	{
		Player[] allPlayers = GameManager.Instance.Players;
		List<Player> otherPlayers = allPlayers.Where(player => player != saboteur).ToList();
		otherPlayers.Sort();

		// Display ranked options & random option, for four options total
		PanelManager.Instance.ShowPanel(_victimSelectPanel);

		// Wait for selection (for sake of implementation, this is how to get a random victim)
		_selectedPlayerIndex = -1;
		yield return new WaitUntil(() => _selectedPlayerIndex != -1);

		// Use the index of the selected victim button to get an actual victim player object
		if (_selectedPlayerIndex == otherPlayers.Count) _selectedPlayerIndex = Random.Range(0, otherPlayers.Count); // Last button counts as random
		Player selectedVictim = otherPlayers[_selectedPlayerIndex];

		// Determine whether or not it's possible for the saboteur to steal oil
		bool canStealOil = selectedVictim.Oil > 0 && saboteur.Coins >= StealingOilCost;

		// Display choice between stealing coins or oil
		PanelManager.Instance.ShowPanel(_methodSelectPanel);

		// Wait for selection (for this, let's just say if they can steal oil, they will steal oil)
		bool wantsOil = canStealOil;

		// Handle the logic for taking away oil or coins depending on selection
		if (wantsOil)
		{
			selectedVictim.Oil--;
			saboteur.Oil++;
		}
		else
		{
			int coinsStolen = Mathf.Max(MaxCoinsToSteal, selectedVictim.Coins);
			selectedVictim.Coins -= coinsStolen;
			saboteur.Coins += coinsStolen;
		}

		// Show the sabotage consequence
		PanelManager.Instance.ShowPanel(_consequencePanel);

		// Wait for consequence to be canceled out of

		yield break;
	}
}
