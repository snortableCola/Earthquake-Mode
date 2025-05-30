using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

[RequireComponent(typeof(VisibleTag), typeof(PlayerMovement))]
public class Player : MonoBehaviour, IComparable<Player>
{
	[SerializeField] int playerIndex = -1;
    [SerializeField] Item cheapItemHacked;

	[SerializeField] private TextMeshProUGUI _profileCoinsDisplay;
	[SerializeField] private TextMeshProUGUI _profileOilDisplay;
	[SerializeField] Canvas canvas;
    public VisibleTag FrozenTag { get; private set; }
	public PlayerMovement Movement { get; private set; }
	public Item UsedItem { get; set; } = null;


	public Item HeldItem { get; set; } = null;
	public int PlayerIndex
	{
		get { return playerIndex; }
    }

	public MultiplayerEventSystem multiplayerEventSystem;
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
		// we need to makae sure that there are only as many players as there are player inputs
		playerInput = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None).Where(x => x.playerIndex == this.PlayerIndex).First();
		if(playerInput == null)
		{
			Destroy(this);
		}
		playerInput.transform.GetChild(0).gameObject.SetActive(true);
		
		multiplayerEventSystem = playerInput.GetComponentInChildren<MultiplayerEventSystem>();
		if (multiplayerEventSystem == null)
		{
			Debug.LogError("no eventsystem");
		}
		playerInput.transform.GetChild(0).gameObject.SetActive(false);
        //initialize canvas as player root
        //multiplayerEventSystem.playerRoot = canvas.gameObject;
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