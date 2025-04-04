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
	public int totalPoints = 100; // Player's total points

	// Method to adjust points
	public void AdjustPoints(int points)
    {
        Debug.Log($"Adjusting points for {name}. Current points: {totalPoints}, Adjust by: {points}");
        totalPoints += points;
        Debug.Log($"New points for {name}: {totalPoints}");
    }
	#endregion
}