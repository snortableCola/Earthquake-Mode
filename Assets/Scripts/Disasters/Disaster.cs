using UnityEngine;

public abstract class Disaster : MonoBehaviour
{
    public abstract bool IsPossible { get; }

    public abstract void StartDisaster(Player incitingPlayer);

    public abstract void Refresh();

	private void Start()
	{
		Refresh();
	}
}
