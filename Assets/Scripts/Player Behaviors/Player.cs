using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(VisibleTag), typeof(PlayerMovement))]
public class Player : MonoBehaviour, IComparable<Player>
{
	[SerializeField] int playerIndex = -1;
    [SerializeField] Item cheapItemHacked;

	[SerializeField] private TextMeshProUGUI _profileCoinsDisplay;
	[SerializeField] private TextMeshProUGUI _profileOilDisplay;

	public VisibleTag FrozenTag { get; private set; }
	public PlayerMovement Movement { get; private set; }
	public Item UsedItem { get; set; } = null;

	public Item[] HeldItems = new Item[2];
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

	private int _coins = 10; // Player's total coins
	private int _oil; // Player's collected oil
	public int Coins
	{
		get => _coins;
		set
		{
			_profileCoinsDisplay.text = value.ToString();
			_coins = value;
		}
	}

	public int Oil
	{
		get => _oil;
		set
		{
			_profileOilDisplay.text = value.ToString();
			_oil = value;
		}
	}

	public int CompareTo(Player other)
	{
		int oilComparison = other.Oil.CompareTo(Oil);
		return oilComparison == 0 ? other._coins.CompareTo(_coins) : oilComparison;
	}
}