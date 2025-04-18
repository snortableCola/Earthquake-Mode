using System;
using UnityEngine;

[RequireComponent(typeof(VisibleTag), typeof(PlayerMovement))]
public class Player : MonoBehaviour, IComparable<Player>
{
	public VisibleTag FrozenTag { get; private set; }
	public PlayerMovement Movement { get; private set; }
	public Item UsedItem { get; set; } = null;
	public Item HeldItem { get; set; } = null;

	[SerializeField] Item cheapItemHacked;

	public void Awake()
	{
		FrozenTag = GetComponent<VisibleTag>();
		Movement = GetComponent<PlayerMovement>();
		HeldItem = cheapItemHacked;
	}

	/// <summary>
	/// The biome of the space which the player is currently occupying.
	/// </summary>
	public Biome CurrentBiome => transform.GetComponentInParent<Space>().Biome;

	private int _coins = 100; // Player's total coins
	public int _oil = 0; // Player's collected oil

	public int Coins
	{
		get => _coins;
		set
		{
			Debug.Log($"Adjusting coins for {name}. Current coins: {_coins}, Change to: {value}");
			_coins = value;
		}
	}

	public int CompareTo(Player other)
	{
		int oilComparison = other._oil.CompareTo(_oil);
		return oilComparison == 0 ? other._coins.CompareTo(_coins) : oilComparison;
	}
}