using UnityEngine;

[RequireComponent(typeof(VisibleTag), typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
	public VisibleTag FrozenTag { get; private set; }
	public PlayerMovement Movement { get; private set; }

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
	public void AdjustPoints(int amount)
    {
        totalPoints += amount;
    }
	#endregion
}