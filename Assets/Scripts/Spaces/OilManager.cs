using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OilManager : MonoBehaviour
{
	public static OilManager Instance { get; private set; }

	[SerializeField] private int ActiveSpaceCount = 2;
	private OilSpace[] _oilSpaces;

	[SerializeField] private GameObject _oilPanel;
	[SerializeField] private Button _buyButton;
	[SerializeField] private Button _passButton;
	[SerializeField] private TextMeshProUGUI _prompt;
	private bool? _buyingOil;

	private void Awake()
	{
		Instance = this;
		_oilSpaces = FindObjectsByType<OilSpace>(FindObjectsSortMode.None);
		ActivateRandomOilSpaces();

		_passButton.onClick.AddListener(() => _buyingOil = false);
		_buyButton.onClick.AddListener(() => _buyingOil = true);
	}

	public IEnumerator PurchaseProcess(Player customer, int cost)
	{
		_prompt.text = $"You may purchase 1 oil for {cost} coins!\nYou have {customer.Coins} coins.";
		_buyButton.interactable = customer.Coins >= cost;

		PanelManager.Instance.ShowPanel(_oilPanel);

		_buyingOil = null;
		yield return new WaitUntil(() => _buyingOil != null);

		if (_buyingOil == true)
		{
			customer.Coins -= cost;
			customer.Oil += cost;
		}

		ActivateRandomOilSpaces();

		yield return DisasterManager.Instance.IncrementEarthquake();

		PanelManager.Instance.ShowMovementUI();
	}

	public void ActivateRandomOilSpaces()
	{
		for (int i = _oilSpaces.Length - 1; i > 0; i--)
		{
			int rnd = Random.Range(0, i + 1);
			(_oilSpaces[i], _oilSpaces[rnd]) = (_oilSpaces[rnd], _oilSpaces[i]);
		}
		for (int i = 0; i < _oilSpaces.Length; i++)
		{
			_oilSpaces[i].IsActive = i < ActiveSpaceCount;
		}
	}
}
