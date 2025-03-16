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

	#region Movement Logic
	[SerializeField] private float _movementTime;
	[SerializeField] private float _jumpHeight;
	[SerializeField] private float _movementDelay;

	/// <summary>
	/// Coroutine that makes the player move a random number of spaces along the board (if possible), triggering the destination's landing behavior.
	/// </summary>
	/// <returns>Coroutine for handling the player's movement.</returns>
	public IEnumerator RandomMovementCoroutine()
	{
		int distance = Random.Range(1, 7); // Moves random distance 1-6 to emulate dice roll
		Debug.Log($"{this} moves {distance} spaces.");

		yield return new WaitForSeconds(_movementDelay); // Initial delay for movement just so the first jump doesn't feel unusually fast

		Space space = transform.GetComponentInParent<Space>();
		while (distance > 0)
		{
			space = space.GetComponent<NextSpaceProvider>().NextSpace;

			SpaceBehavior behavior = space.GetComponent<SpaceBehavior>();

			// If the next space can be passed, it doesn't decrement player movement
			if (!behavior.EndsTurn)
			{
				yield return JumpToSpaceCoroutine(space, false);
				behavior.RespondToPlayer(this); // Trigger the space's passing behavior
				continue;
			}

			yield return JumpToSpaceCoroutine(space, --distance == 0); // Only trigger the space's landing behavior if it's the final space
		}
	}

	/// <summary>
	/// Coroutine that makes the player jump from their current space to a specified destination space.
	/// </summary>
	/// <param name="targetSpace">The target space for the player to jump to.</param>
	/// <param name="triggerLandingBehavior">If true, triggers the corresponding behavior of the target space upon the player's landing.</param>
	/// <returns>Coroutine for handling the player's jump.</returns>
	public IEnumerator JumpToSpaceCoroutine(Space targetSpace, bool triggerLandingBehavior)
	{
		Vector3 startPosition = transform.position;
		Vector3 endPosition = targetSpace.transform.TransformPoint(transform.localPosition); // Ensures the player's located identically relative to its space after landing

		float timeElapsed = 0f;
		while (timeElapsed < _movementTime)
		{
			timeElapsed += Time.deltaTime; // Movement is frame-independent
			
			float movementProgress = timeElapsed / _movementTime;
			Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, movementProgress);
			currentPosition.y += _jumpHeight * movementProgress * (1 - movementProgress); // Player follows quadratic trajectory

			transform.position = currentPosition;

			yield return null; // Movement iterates frame-by-frame
		}

		transform.position = endPosition;
		transform.SetParent(targetSpace.transform);

		if (!triggerLandingBehavior) yield break;

		if (targetSpace.BurningTag.State) // Fire takes precendent over default space behavior
		{
			Debug.Log($"{this} landed on a space which is on fire.");
		}
		else
		{
			SpaceBehavior behavior = targetSpace.GetComponent<SpaceBehavior>();
			behavior.RespondToPlayer(this);
		}
	}
	#endregion
}