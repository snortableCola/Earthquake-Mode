using UnityEngine;

[RequireComponent(typeof(VisibleTag), typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
	public string playerName; 
	public VisibleTag FrozenTag { get; private set; }
	public PlayerMovement Movement { get; private set; }
	public IItem UsedItem { get; set; } = null;

	public void Awake()
	{
		FrozenTag = GetComponent<VisibleTag>();
		Movement = GetComponent<PlayerMovement>();
	}

	/// <summary>
	/// The biome of the space which the player is currently occupying.
	/// </summary>
	public Biome CurrentBiome => transform.GetComponentInParent<Space>().Biome;

	#region Points Logic
	private int _points = 100; // Player's total points
	public int Points
	{
		get => _points;
		set
		{
			Debug.Log($"Adjusting points for {name}. Current points: {_points}, Change to: {value}");
			_points = value;
		}
	}
	#endregion
}