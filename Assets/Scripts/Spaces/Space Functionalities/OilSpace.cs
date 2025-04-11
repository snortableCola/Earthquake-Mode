using System.Collections;
using UnityEngine;

public class OilSpace : ResourceSpace
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

	public override bool EndsTurn => !_isActive;

	[ContextMenu("Flip Oil Space State")]
	public void FlipActivity() => IsActive = !_isActive;

	public override IEnumerator RespondToPlayer(Player player)
	{
		if (_isActive)
		{
			Debug.Log($"{player.name} passed on an active oil space.");

			yield return DisasterManager.Instance.IncrementEarthquake();
		}

		else
		{
			Debug.Log($"{player.name} landed on an inactive oil space.");

			yield return base.RespondToPlayer(player);
		}
	}
}