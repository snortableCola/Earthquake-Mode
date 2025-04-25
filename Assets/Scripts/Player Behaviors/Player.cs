using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(VisibleTag), typeof(PlayerMovement))]
public class Player : MonoBehaviour, IComparable<Player>
{
	[SerializeField] int playerIndex = -1;
    [SerializeField] Item cheapItemHacked;
	public VisibleTag FrozenTag { get; private set; }
	public PlayerMovement Movement { get; private set; }
	public Item UsedItem { get; set; } = null;
	public Item HeldItem { get; set; } = null;
	public int PlayerIndex
	{
		get { return playerIndex; }
    }

	public PlayerInput playerInput { get; private set; } = null;


    public void Awake()
	{
        
        Debug.Log($"Awake called for {gameObject.name}");
		//used to be plugged in to all characters, but since we're changing to using prefabs for players, we can't do that anymore
		//cheapItemHacked = FindFirstObjectByType<SpaceSwap>();
		FrozenTag = GetComponent<VisibleTag>();
		Movement = GetComponent<PlayerMovement>();
		HeldItem = cheapItemHacked;
	}

    public void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    /// <summary>
    /// The biome of the space which the player is currently occupying.
    /// </summary>
    public Biome CurrentBiome => transform.GetComponentInParent<Space>().Biome;

	private int _coins = 100; // Player's total coins
	public int Oil { get; set; } // Player's collected oil

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
		int oilComparison = other.Oil.CompareTo(Oil);
		return oilComparison == 0 ? other._coins.CompareTo(_coins) : oilComparison;
	}
}