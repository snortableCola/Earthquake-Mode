using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GeneralResourceSpace : MonoBehaviour
{
	public static GeneralResourceSpace Instance { get; private set; }

	[SerializeField] private Image _blueHUD;
	public float _hudMessageDuration = 2f;

	private void Awake()
	{
		Instance = this;
	}

	public IEnumerator RespondToPlayerEnd(Player player)
	{
		Biome biome = player.CurrentBiome;
		Debug.Log($"{player.name} landed on a {biome} resource space.");
		_blueHUD.gameObject.SetActive(true);
		yield return new WaitForSeconds(_hudMessageDuration);
		_blueHUD.gameObject.SetActive(false);
		player.Coins += 3;

		yield return DisasterManager.Instance.IncrementBiomeDisaster(biome, player);
	}
	public IEnumerator WaitForHudCompletion()
	{
		while (_blueHUD != null && _blueHUD.gameObject.activeSelf)
		{
			yield return null; // Wait until the red HUD is no longer active
		}
	}
}
