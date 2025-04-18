using System.Collections;
using UnityEngine;

public class OilSpace : SpaceBehavior
{
	private readonly Vector3 _activeScale = new(0.2f, 0.05f, 0.2f);
	private readonly Vector3 _inactiveScale = new(0.35f, 0.05f, 0.35f);

	private bool _isActive;
	public bool IsActive
	{
		get => _isActive;
		set
		{
			_isActive = value;
			transform.localScale = value ? _activeScale : _inactiveScale;
		}
	}

	public override bool HasPassingBehavior => _isActive;

	[ContextMenu("Flip Oil Space State")]
	public void FlipActivity() => IsActive = !_isActive;

	public override IEnumerator RespondToPlayerPassing(Player player)
	{
		Debug.Log($"{player.name} passes an active oil space. They may purchase oil without a discount.");

		yield return PurchaseOil(player);
	}

	public override IEnumerator RespondToPlayerEnd(Player player)
	{
		if (_isActive)
		{
			Debug.Log($"{player.name} landed on an active oil space. They may purchase oil with a discount.");

			yield return PurchaseOil(player);
		}

		else
		{
			Debug.Log($"{player.name} landed on an inactive oil space. Will function as resource space.");

			yield return GeneralResourceSpace.Instance.RespondToPlayerEnd(player);
		}
	}

	private IEnumerator PurchaseOil(Player player)
	{
		player.transform.parent = null;
		OilManager.Instance.SelectRandomOilSpaces();
		player.transform.parent = transform;

		yield return DisasterManager.Instance.IncrementEarthquake();
	}

	public override IEnumerator WaitForHudCompletion() => GeneralResourceSpace.Instance.WaitForHudCompletion();
}