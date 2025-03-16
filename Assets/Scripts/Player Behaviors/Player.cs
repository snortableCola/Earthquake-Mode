using System.Collections;
using UnityEngine;

[RequireComponent(typeof(VisibleTag), typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
	[HideInInspector] public VisibleTag FrozenTag;
	[HideInInspector] public PlayerMovement Movement;

	public void Awake()
	{
		FrozenTag = GetComponent<VisibleTag>();
		Movement = GetComponent<PlayerMovement>();
	}

	/// <summary>
	/// The biome of the space which the player is currently occupying.
	/// </summary>
	public Space.BoardBiome CurrentBiome => transform.GetComponentInParent<Space>().Biome;

	#region Points Logic
	public int totalPoints = 100; // Player's total points

	// Method to adjust points
	public void AdjustPoints(int amount)
    {
        totalPoints += amount;
    }
	#endregion
}