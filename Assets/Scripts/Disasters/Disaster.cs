using System.Collections;
using UnityEngine;

public abstract class Disaster : MonoBehaviour
{
	public virtual bool IsPossible { get; } = true;

    public abstract IEnumerator StartDisaster(Player incitingPlayer);

    public virtual void Refresh() { }

	private void Start()
	{
		Refresh();
	}
}
