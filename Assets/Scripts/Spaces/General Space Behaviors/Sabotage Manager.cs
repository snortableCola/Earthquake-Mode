using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SabotageManager : MonoBehaviour
{
	public static SabotageManager Instance { get; private set; }

	[SerializeField] private GameObject _playerSelectPanel;
	[SerializeField] private GameObject _methodSelectPanel;
	[SerializeField] private GameObject _consequencePanel;

	[SerializeField] private int MaxCoinsToSteal;
	[SerializeField] private int StealingOilCost;

	private void Awake()
	{
		Instance = this;
	}

	public IEnumerator Sabotage(Player saboteur)
	{
		Player[] allPlayers = GameManager.Instance.Players;
		List<Player> otherPlayers = allPlayers.Where(player => player != saboteur).ToList();
		otherPlayers.Sort();

		// Display ranked options & random option, for four options total
		PanelManager.Instance.ShowPanel(_playerSelectPanel);

		// Wait for selection (for sake of implementation, this is how to get a random victim)
		Player selectedVictim = otherPlayers[Random.Range(0, otherPlayers.Count)];

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
