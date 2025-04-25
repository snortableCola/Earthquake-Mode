using System.Collections;
using UnityEngine;

public class OilSpace : SpaceBehavior
{
	private static readonly Vector3 s_activeScale = new(0.2f, 0.05f, 0.2f);
	private static readonly Vector3 s_inactiveScale = new(0.35f, 0.05f, 0.35f);

	private bool _isActive;
	public bool IsActive
	{
		get => _isActive;
		set
		{
			_isActive = value;
			transform.localScale = value ? s_activeScale : s_inactiveScale;
		}
	}

	public override bool HasPassingBehavior => _isActive;

	[ContextMenu("Flip Oil Space State")]
	public void FlipActivity() => IsActive = !_isActive;

	public override IEnumerator RespondToPlayerPassing(Player player) => OilManager.Instance.PurchaseProcess(player, 15);

	public override IEnumerator RespondToPlayerEnd(Player player)
	{
		if (_isActive)
		{
			return OilManager.Instance.PurchaseProcess(player, 12);
		}
		else
		{
			return GeneralResourceSpace.Instance.RespondToPlayerEnd(player);
		}
	}

	public override IEnumerator WaitForHudCompletion() => GeneralResourceSpace.Instance.WaitForHudCompletion();
}